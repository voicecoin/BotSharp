function isIE(){
        if (!!window.ActiveXObject || "ActiveXObject" in window)
        return true;
        else
        return false;
}
var httpCount = 0;
function getKey(){
  const options={
      url: '/api/Registry/SiteSettings',
      method: 'GET',
      async: true,
      timeout: 0,
      data: null,
      dataType: 'json',
      headers: {'authorization':'Bearer ' + localStorage.getItem('access_token')},
      onprogress: function () { },
      onuploadprogress: function () { },
      xhr: null
  }
    var xhr = options.xhr = options.xhr || new XMLHttpRequest();

    return new Promise(function(resolve, reject){
        xhr.open(options.method,options.url,options.async);
        xhr.timeout = options.timeout;
        for(let k in options.headers){
            xhr.setRequestHeader(k,options.headers[k]);
        }
        xhr.onprogress = options.onprogress;
        xhr.upload.onprogress = options.onuploadprogress;
        xhr.responseType = options.dataType;

        xhr.onloadstart=function(){
        }
        xhr.onabort = function () {
            reject(new Error({
                errorType: 'abort_error',
                xhr: xhr
            }));
        }
        xhr.ontimeout = function () {
            reject({
                errorType: 'timeout_error',
                xhr: xhr
            });
        }
        xhr.onerror = function () {
            reject({
                errorType: 'onerror',
                xhr: xhr
            })
        }
        xhr.onloadend = function () {
            --httpCount
            if ((xhr.status >= 200 && xhr.status < 300) || xhr.status === 304){
                let state = isIE();
                if(state == true){
                    resolve(JSON.parse(xhr.response));
                }else{
                    resolve(xhr.response);
                }
            }else{
                reject({
                    errorType: 'status_error',
                    xhr: xhr
                })
            }
            if(httpCount == 0){
                NProgress.done();
            }
        }
        try {
          if(options.headers['Content-Type'] == 'application/x-www-form-urlencoded'){
            xhr.send(options.transformRequest(options.data));
          }
          else xhr.send(JSON.stringify(options.data));
        }
        catch (e) {
            reject({
                errorType: 'send_error',
                error: e
            });
        }
    })
}
