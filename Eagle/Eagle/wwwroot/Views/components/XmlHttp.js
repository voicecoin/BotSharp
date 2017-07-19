import NProgress from 'nprogress'
import 'nprogress/nprogress.css'
const ajaxOptions={
    url: '#',
    method: 'GET',
    async: true,
    timeout: 0,
    data: null,
    dataType: 'json',
    headers: {'authorization':'Bearer ' + localStorage.getItem('access_token')},
    onprogress: function () { },
    onuploadprogress: function () { },
    xhr: null,
    transformRequest: function(obj) {
        var str = [];
        for(var p in obj)
        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
        return str.join("&");
    }
}
let httpCount = 0
export class Http {

    HttpAjax(optionsOverride){
        let self = this;
        if(!httpCount){
            NProgress.start();
        }
        ++httpCount
        let options={};
        for (var k in ajaxOptions) {
            options[k] = optionsOverride[k] || ajaxOptions[k];
        }
        options.async = options.async === false ? false : true;
        var xhr = options.xhr = options.xhr || new XMLHttpRequest();
        return new Promise(function(resolve, reject){
            xhr.open(options.method,options.url,options.async);
            xhr.timeout = options.timeout;

            //设置请求头
            for(let k in options.headers){
                xhr.setRequestHeader(k,options.headers[k]);
            }

            //注册xhr对象事件
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
                    let state = self.isIE();
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


     creatXMLHttpRequest(){
      if(window.XMLHttpRequest){ //判定兼容性
        return new XMLHttpRequest(); //Dom2浏览器
      }
      else if(window.ActiveXObject){  //IE浏览器
        try{
          return new ActiveXObject("msxml2.XMLHTTP");
        }catch(e){
          try{
            return new ActiveXObject("Microsoft.XMLHTTP");
          }catch(e){}
        }
      }
    }
    isIE(){
            if (!!window.ActiveXObject || "ActiveXObject" in window)
            return true;
            else
            return false;
    }
    //格式化参数

}

export default Http
