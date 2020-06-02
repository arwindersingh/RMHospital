Usage
#####

Go to the `mock-api` directory and run the mock api by doing the following
command 

    $ node server.js

The mock api server will be available in port 3333 by default, you can change
this behavior by editing the `server.js`.


**Note**

This mock api which supports following types of request. It looks for request
body if the request method is PUT. It then saves the request body. Any
subsequent GET request will return the updated response.

    GET /api/pod/
    GET /api/pod/<id>/
    PUT /api/pod/<id>/


