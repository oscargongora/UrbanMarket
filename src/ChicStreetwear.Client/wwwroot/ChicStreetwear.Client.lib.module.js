window.preventSubmitFormOnEnter = () => {
    const forms = document.getElementsByClassName("preventSubmitOnEnter");
    Array.from(forms).forEach(form => {
        form.addEventListener('keydown', function (event) {
            if (event.code == "Enter" || event.code == "NumpadEnter") {
                event.preventDefault();
                return false;
            }
        });
    })
}






