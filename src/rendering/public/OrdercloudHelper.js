function Ajax(url, success) {
  var xhr = window.XMLHttpRequest ? new XMLHttpRequest() : new ActiveXObject('Microsoft.XMLHTTP');
  xhr.open('GET', url);
  xhr.onreadystatechange = function () {
    if (xhr.readyState > 3 && xhr.status == 200) success(xhr.responseText);
  };
  xhr.setRequestHeader('X-Requested-With', 'XMLHttpRequest');
  xhr.send();
  return xhr;
}

function PlaceMessage(message) {
  var messageContainer = document.getElementById('AjaxResponse');
  messageContainer.innerHTML = message;
}

function InitializeSelectVariants() {
  var sel = document.getElementById('variants');
  if(sel){
    AssignProductImage(sel);
    sel.addEventListener('change', function (e) {
        AssignProductImage(e.target);
    });
  }
}

function AssignProductImage(element) {
  var pImage = document.getElementById('productImage');
  if(pImage){
    var variant = document.getElementById(element.value);
    var variantImage = variant.getAttribute('img-url');
    if(variantImage){
      pImage.setAttribute('src', variantImage);
    }
  }
}
