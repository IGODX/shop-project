// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
    function changeMainPhoto(id, photoPath){
    let mainPhoto = document.getElementById('mainPhoto');
    mainPhoto.setAttribute('src', `${photoPath}`);
    mainPhoto.setAttribute('data-main-img-id', `${id}`);
    return false;
        }