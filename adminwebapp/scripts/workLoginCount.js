onmessage = function(e){

    var xhr = new XMLHttpRequest();  
    xhr.open("GET", "../Bonsa/Services/GetLoginCount.aspx");  
    xhr.onload = function(){  
        postMessage(xhr.responseText);  
    };  
    xhr.send();  
};