// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function ConfirmDelete(UniqueId, IsDeleteClicked) {
    var Delete = 'DeleteSpan_' + UniqueId;
    var ConfirmDelete = 'confirmSpan_' + UniqueId;
    if (IsDeleteClicked) {
        $('#' + Delete).hide();
        $('#' + ConfirmDelete).show();
    }
    else {
        $('#' + Delete).show();
        $('#' + ConfirmDelete).hide();
    }
}
