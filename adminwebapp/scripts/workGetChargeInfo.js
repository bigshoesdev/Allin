/* 회원 충환전 데이터 불러 온다 */
onmessage = function(e){

    var xhr = new XMLHttpRequest();  
    xhr.open("GET", "../Bonsa/Services/Notify.aspx");  
    xhr.onload = function(){  
        postMessage(xhr.responseText);  
    };  
    xhr.send();  
};