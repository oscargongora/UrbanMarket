using Blazored.FluentValidation;
using ChicStreetwear.Client.Helpers;
using ChicStreetwear.Client.Pages.Product.Pictures;
using ChicStreetwear.Client.Services;
using ChicStreetwear.Shared.Models.Category;
using ChicStreetwear.Shared.Models.Common;
using ChicStreetwear.Shared.Models.Product;
using ChicStreetwear.Shared.Options;
using ChicStreetwear.Shared.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;
using System.Globalization;
using System.Net.Http.Json;
using System.Reflection.Metadata;

namespace ChicStreetwear.Client.Pages.Product;
public sealed class ProductFormModel : ProductModel
{
}
public sealed class ProductFormModelValidator : AbstractValidator<ProductFormModel>
{
    public ProductFormModelValidator()
    {
        Include(new ProductModelValidator());
    }
}
public partial class Form
{
    [Inject]
    public ISnackbar _snackbar { get; set; } = default!;
    [Inject]
    public IDialogService _dialogService { get; set; } = default!;
    [Inject]
    public HttpClient _httpClient { get; set; } = default!;
    [Inject]
    internal FileService _fileService { get; set; } = default!;
    [Inject]
    public NavigationManager _navigation { get; set; } = default!;

    [Parameter]
    public Guid? Id { get; set; }


    private ProductFormModel? Model = new();
    private EditContext? EditContext;
    private FluentValidationValidator? _fluentValidationValidator;

    private RoutingModel Routing = RoutingHelper.Product;

    private List<CategoryModel> categories = new();
    private ICollection<object> selectedCategories = new HashSet<object>();

    private bool isLoadingFiles;
    private List<IBrowserFile> selectedFiles = new();
    private List<string> selectedFilesUrls = new();

    private CultureInfo EnUs = CultureInfo.GetCultureInfo("en-US");

    private bool isLoading;
    private void SetIsLoading() => isLoading = true;
    private void UnSetIsLoading() => isLoading = false;

    #region initialization
    protected override async Task OnInitializedAsync()
    {
        SetIsLoading();
        await LoadCategories();
        if (Id is not null)
        {
            await LoadModel();
        }
        EditContext = new(Model!);
        UnSetIsLoading();
    }

    private async Task LoadModel()
    {
        var response = await _httpClient.GetAsync(Routing.GetByIdApiEndpoint((Guid)Id));
        if (response.IsSuccessStatusCode)
        {
            Model = await response.Content.ReadFromJsonAsync<ProductFormModel>();
            if (Model is not null)
            {
                selectedCategories = new HashSet<object>(Model.Categories.Select(c => (object)c.CategoryId));
            }
        }
        else
        {
            _navigation.NavigateTo(Routing.ListPage);
            _snackbar.Add(await response.Content.ReadAsStringAsync(), MudBlazor.Severity.Error);
        }
    }
    private async Task LoadCategories()
    {
        categories = (await _httpClient.GetFromJsonAsync<List<CategoryModel>>(RoutingHelper.Category.ApiEndpoint)) ?? new();
    }
    #endregion

    #region submission
    private async Task OnValidSubmitAsync()
    {
        if (await _fluentValidationValidator.ValidateAsync())
        {
            try
            {
                LoadCategoriesToSubmit();

                if (Model.HasAttributes)
                {
                    Model.Pictures.Clear();
                    selectedFiles.Clear();
                    selectedFilesUrls.Clear();
                    Model.CoverPicture = null;
                }
                else
                {
                    Model.Attributes.Clear();
                    Model.Variations.Clear();
                }

                if (selectedFiles.Any())
                {
                    await LoadFilesToSubmit();
                }

                var response = Id is null ?
                await _httpClient.PostAsJsonAsync(Routing!.ApiEndpoint, Model) :
                await _httpClient.PutAsJsonAsync(Routing!.PutApiEndpoint((Guid)Id), Model);

                if (response.IsSuccessStatusCode)
                {
                    EditContext!.MarkAsUnmodified();
                    if (Id is null)
                    {
                        var id = await response.Content.ReadFromJsonAsync<Guid>();
                        _navigation.NavigateTo(Routing.EditPage(id));
                    }
                    _snackbar.Add($"{Routing.EntityName} saved", MudBlazor.Severity.Success);
                }
                else
                {
                    _snackbar.Add(await response.Content.ReadAsStringAsync(), MudBlazor.Severity.Error);
                }

            }
            catch (Exception exception)
            {
                _snackbar.Add(exception.Message, MudBlazor.Severity.Error);
            }
        }
    }
    private async Task LoadFilesToSubmit()
    {
        var picturesResult = await _fileService.UploadPictures(selectedFiles);
        foreach (var pr in picturesResult)
        {
            if (!pr.HasErrors)
            {
                Model.Pictures.Add(pr.Data);
                if (Model.CoverPicture is not null && Model.CoverPicture.Name.Equals(pr.Data.Name) && Model.CoverPicture.Url.StartsWith("data"))
                {
                    Model.CoverPicture = pr.Data;
                }
            }
            else
            {
                _snackbar.Add(pr.Errors.FirstOrDefault()?.Message, MudBlazor.Severity.Error);
            }
        }
        selectedFiles.Clear();
        selectedFilesUrls.Clear();
    }
    private void LoadCategoriesToSubmit()
    {
        foreach (var category in Model.Categories.ToList())
        {
            if (!selectedCategories.Any(sc => category.CategoryId.Equals((Guid)sc)))
            {
                Model.Categories.RemoveAll(c => c.CategoryId.Equals(category.CategoryId));
            }
        }
        foreach (var categoryId in selectedCategories)
        {
            Model.Categories.Add(new() { CategoryId = (Guid)categoryId });
        }
    }
    #endregion

    #region events
    private async Task OnDeleteClick()
    {
        var isConfirmed = await _dialogService.ShowMessageBox("Warning", "Are you sure you want to delete this product?", "Yes", "No");

        if (isConfirmed is true && Id is not null)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(Routing!.GetByIdApiEndpoint((Guid)Id));
                if (response.IsSuccessStatusCode)
                {
                    EditContext!.MarkAsUnmodified();
                    _navigation.NavigateTo(Routing.ListPage);
                    _snackbar.Add($"{Routing!.EntityName} deleted", MudBlazor.Severity.Success);
                }
            }
            catch (Exception exception)
            {
                _snackbar.Add(exception.Message, MudBlazor.Severity.Error);
            }
        }
    }
    private async Task OnBeforeInternalNavigation(LocationChangingContext locationChanging)
    {
        if (EditContext!.IsModified())
        {
            var isConfirmed = await _dialogService.ShowMessageBox("Warning", "Changes you made may not be saved. Do you want to leave the page?", "Yes", "No");
            if (isConfirmed is false)
            {
                locationChanging.PreventNavigation();
            }
        }
    }
    #endregion

    #region attributes
    private void SaveAttribute(string? name, AttributeModel attribute)
    {
        var _attribute = Model.Attributes.FirstOrDefault(_a => _a.Name.Equals(name));

        if (_attribute is null)
        {
            Model.Attributes.Add(attribute);
        }
        else
        {
            foreach (var variation in Model.Variations)
            {
                foreach (var av in variation.Attributes)
                {
                    if (av.Attribute.Name.Equals(_attribute.Name))
                    {
                        av.Attribute.Name = attribute.Name;
                    }
                }
            }
            _attribute.Name = attribute.Name;
        }
    }
    private async Task RemoveAttribute(AttributeModel attribute)
    {
        bool? isConfirmed = null;
        if (Model.Attributes.Count == 1)
        {
            isConfirmed = await _dialogService.ShowMessageBox("Warning", "By eliminating the only attribute that the product has, the variations will also be eliminated. Are you sure you want to proceed?", "Yes", "No");
            if (isConfirmed is true)
            {
                Model.Variations.Clear();
            }
        }
        else
        {
            isConfirmed = await _dialogService.ShowMessageBox("Warning", "Removing the attribute removes the corresponding values in the variations. Are you sure you want to delete this attribute?", "Yes", "No");
        }

        if (isConfirmed is true)
        {
            foreach (var v in Model.Variations)
            {
                v.Attributes.RemoveAll(av => av.Attribute.Name.Equals(attribute.Name));
            }
            Model.Attributes.Remove(attribute);
        }
    }
    private void SaveVariation(Guid? id, VariationModel variation)
    {
        var _variation = Model.Variations.FirstOrDefault(_v => _v.Id.Equals(id));

        if (_variation is null)
        {
            Model.Variations.Add(variation);
        }
        else
        {
            _variation.Attributes = variation.Attributes.ToList();
            _variation.Stock = variation.Stock;
            _variation.PurchasedPrice = variation.PurchasedPrice;
            _variation.RegularPrice = variation.RegularPrice;
            _variation.SalePrice = variation.SalePrice;
        }
    }
    private async Task RemoveVariation(VariationModel variation)
    {
        var isConfirmed = await _dialogService.ShowMessageBox("Warning", "Are you sure you want to delete this variation?", "Yes", "No");
        if (isConfirmed is true)
        {
            Model.Variations.Remove(variation);
        }
    }
    #endregion

    #region pictures
    int PicturesCount => (Model?.Pictures?.Count ?? 0) + (selectedFiles?.Count ?? 0);
    string PicturesSectionClass => PicturesCount == 0 ? "no-children" : string.Empty;
    private void SetIsLoadingFiles() => isLoadingFiles = true;
    private void UnSetIsLoadingFiles() => isLoadingFiles = false;
    private async Task OnFilesChanged(InputFileChangeEventArgs eventArgs)
    {
        SetIsLoadingFiles();

        try
        {
            if (Model.Pictures.Count < PicturesOptions.MaxFileCount)
            {
                selectedFiles = (await Task.WhenAll(eventArgs.GetMultipleFiles(PicturesOptions.MaxFileCount - Model.Pictures.Count).Select(async f => await f.RequestImageFileAsync(f.ContentType, PicturesOptions.MaxWidth, PicturesOptions.MaxHeight)))).ToList();

                selectedFilesUrls = (await Task.WhenAll(selectedFiles.Select(async sf => await FileHelper.ReadImageFromFile(sf)))).ToList();
            }
            else
            {
                _snackbar.Add($"A product can only have {PicturesOptions.MaxFileCount} pictures", MudBlazor.Severity.Error);
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowMessageBox("Error", ex.Message);
        }

        UnSetIsLoadingFiles();
    }
    private async Task OnSelectedPictureClick(string fileUrl)
    {
        var index = selectedFilesUrls.IndexOf(fileUrl);
        var file = selectedFiles.ElementAt(index);

        var modelUrl = await FileHelper.ReadImageFromFile(file, false);
        PictureModel picture = new() { FileName = file.Name, Name = file.Name, Url = modelUrl, ThumbnailUrl = modelUrl };
        var result = await ShowPictureDialog(picture);

        if (!result.Canceled)
        {
            var resultData = (string)result.Data;
            if (resultData.Equals(PictureDialog.SetCoverPictureName))
            {
                Model.CoverPicture = picture;
            }
            if (resultData.Equals(PictureDialog.OnDeleteClickName))
            {
                if (Model?.CoverPicture is not null && Model.CoverPicture.Url.Equals(selectedFilesUrls[index]))
                {
                    Model.CoverPicture = null;
                }
                selectedFilesUrls.RemoveAt(index);
                selectedFiles.RemoveAt(index);
            }
        }
    }
    private async Task OnPictureModelClick(PictureModel picture)
    {
        var result = await ShowPictureDialog(picture);
        if (!result.Canceled)
        {
            var resultData = (string)result.Data;

            if (resultData.Equals(PictureDialog.SetCoverPictureName))
            {
                Model.CoverPicture = picture;
            }
            if (resultData.Equals(PictureDialog.OnDeleteClickName))
            {
                var isConfirmed = await _dialogService.ShowMessageBox("Warning", "Are you sure you want to delete this picture?", "Yes", "No");

                if (isConfirmed is true && Id is not null)
                {
                    try
                    {
                        var response = await _httpClient.DeleteAsync($"{Routing.GetByIdApiEndpoint((Guid)Id)}/pictures/{picture.FileName}");

                        if (response.IsSuccessStatusCode)
                        {
                            if (Model.CoverPicture is not null && Model.CoverPicture.Url.Equals(picture.Url))
                            {
                                Model.CoverPicture = null;
                            }
                            Model.Pictures.Remove(picture);
                            _snackbar.Add($"Picture {picture.Name} deleted", MudBlazor.Severity.Success);
                        }
                    }
                    catch (Exception exception)
                    {
                        _snackbar.Add(exception.Message, MudBlazor.Severity.Error);
                    }
                }
            }
        }
    }
    private async Task<DialogResult> ShowPictureDialog(PictureModel picture)
    {
        var parameters = new DialogParameters { ["Model"] = picture };
        var options = new DialogOptions()
        {
            CloseButton = true,
            FullScreen = true,
            NoHeader = true
        };

        var dialog = await _dialogService.ShowAsync<PictureDialog>(string.Empty, parameters, options);

        return await dialog.Result;
    }
    #endregion
}
