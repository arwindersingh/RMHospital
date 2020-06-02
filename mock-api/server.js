var http = require('http')
var url = require('url')
var fs = require('fs')

var port = 3333;

// Creates a http server which listens on port 3333 by default
http.createServer(function (request, response) {
    var requestUrl = url.parse(request.url); // Request URL
    var method = request.method; // Request Method : GET , POST etc
    var path = requestUrl.path.replace(/\/+$/,''); // Remove trailing / from url
    
    console.log("Request : "+method+" "+path);
    response.writeHead(200,{'Content-Type' : 'application/json'});

    if (method == 'GET'){
        fileContents = getResponseBody(path,method);
        if(fileContents == null)
            fileContents = '{"status":"error"}'; // Default response on error

        response.end(fileContents);
    }

    else if (method == 'PUT'){
        // read the request body
        var requestBody = [];
        request.on('data', function(chunk) {
            requestBody.push(chunk)
        }).on('end', function() {
            responseBody =  handlePut(path,requestBody);    
            if(responseBody == null){
                responseBody = '{"status":"error"}'; // Default response on error
            }
            response.end(responseBody)
            });
    }

}).listen(port);

function handlePut(urlPath, requestBody){
    // return null if the request body was empty or request url does not match
    // the regex 
    var re = new RegExp("^/api/team/(senior|[a-d])$");
    if(requestBody.length != 1 || !re.test(urlPath))
        return null;

    // the last character of request url id the Pod ID
    podID = urlPath.substr(urlPath.length - 1);
    destinationFile = 'jsonresponses/api_pod_'+podID;

    // Update the pod allocation file by the received body
    fs.writeFileSync(destinationFile,requestBody[0]);
    return fs.readFileSync('jsonresponses/put_api_pod_id');
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
    results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    var match =  decodeURIComponent(results[2].replace(/\+/g, " "));
    // console.log(name+" at url "+url+" = "+match);
    return match;
}
function getResponseBody(urlPath){
    // Make sure you we are ignoring the get parameters in the url matching
    urlPart = urlPath.split("?")[0];
    switch(urlPart){
        case '/api/pod':
                return fs.readFileSync('jsonresponses/api_pod');
        break;

        case '/api/profile':
                return fs.readFileSync('jsonresponses/api_nurse_profile');
        break;

        case '/api/deleted_profile':
                return fs.readFileSync('jsonresponses/api_nurse_profile_deleted');
        break;

        case '/api/team/a':
                if(getParameterByName('t', urlPath))
                        return fs.readFileSync('jsonresponses/history/api_pod_a')
                else
                    return fs.readFileSync('jsonresponses/api_pod_a');
        break;

        case '/api/team/b':
                if(getParameterByName('t', urlPath))
                    return fs.readFileSync('jsonresponses/history/api_pod_b')
                else
                    return fs.readFileSync('jsonresponses/api_pod_b');
        break;
        
        case '/api/team/c':
                if(getParameterByName('t', urlPath))
                    return fs.readFileSync('jsonresponses/history/api_pod_c')
                else
                    return fs.readFileSync('jsonresponses/api_pod_c');
        break;

        case '/api/team/d':
                if(getParameterByName('t', urlPath))
                    return fs.readFileSync('jsonresponses/history/api_pod_d')
                else
                    return fs.readFileSync('jsonresponses/api_pod_d');
        break;

        case '/api/team/senior':
                if(getParameterByName('t', urlPath))
                    return fs.readFileSync('jsonresponses/history/api_pod_r')
                else
                    return fs.readFileSync('jsonresponses/api_pod_r');
        break;

        default:
            console.log('Unsupported API request');
            return null;
    }
}

