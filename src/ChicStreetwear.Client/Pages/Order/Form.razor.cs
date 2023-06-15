using Blazored.FluentValidation;
using ChicStreetwear.Client.Helpers;
using ChicStreetwear.Shared.Models.Order;
using ChicStreetwear.Shared.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Net.Http.Json;

namespace ChicStreetwear.Client.Pages.Order;

public sealed class OrderFormModel : OrderModel
{
}
public sealed class OrderFormModelValidator : AbstractValidator<OrderFormModel>
{
    public OrderFormModelValidator()
    {
        Include(new OrderModelValidator());
    }
}

public partial class Form
{
    [Inject]
    private ISnackbar _snackbar { get; set; } = default!;
    [Inject]
    private IDialogService _dialogService { get; set; } = default!;
    [Inject]
    private HttpClient _httpClient { get; set; } = default!;
    [Inject]
    private NavigationManager _navigation { get; set; } = default!;

    [Parameter]
    public Guid? Id { get; set; }

    private OrderFormModel? Model = new();
    private EditContext? EditContext;
    private FluentValidationValidator? _fluentValidationValidator;

    private RoutingModel Routing = RoutingHelper.Order;

    private bool isLoading;
    private void SetIsLoading() => isLoading = true;
    private void UnSetIsLoading() => isLoading = false;

    #region initialization
    protected override async Task OnInitializedAsync()
    {
        SetIsLoading();
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
            Model = await response.Content.ReadFromJsonAsync<OrderFormModel>();
        }
        else
        {
            _navigation.NavigateTo(Routing.ListPage);
            _snackbar.Add(await response.Content.ReadAsStringAsync(), MudBlazor.Severity.Error);
        }
    }
    #endregion

    #region submission
    private async Task OnValidSubmitAsync()
    {
        if (await _fluentValidationValidator.ValidateAsync())
        {
            try
            {
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
    #endregion

    #region events
    private void OnDeleteClick() { return; }

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
}
