import { Error } from 'tslint/lib/error';
import { Url } from 'url';
import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response } from '@angular/http';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class ImageService {

    // The base URL that this service manages
    private static readonly apiUrl: string = '/api/image/';

    // URL of a default image to show if a staff member's ID is not present
    private static readonly defaultImageUrl: string = 'http://s3.amazonaws.com/37assets/svn/765-default-avatar.png';

    // The name to upload the file with (so that ASP.NET Core gets it right)
    private static readonly formDataName: string = 'file';

    /**
     * Construct a new image service
     * @param http Angular's http module
     */
    constructor(private http: Http) {
    }

    /**
     * Get the URL corresponding to a given image ID
     * @param imageId The integer identifier of the image to render as a URL
     */
    public getImageUrlFromId(imageId: number): string {
        if (imageId) {
            return ImageService.apiUrl + imageId;
        }

        return ImageService.defaultImageUrl;
    }

    /**
     * Upload an image file given by a form data style input
     * @param imgFile the image file to upload
     * @return a promise of the ID of the image uploaded
     */
    public uploadImage(imgFile: File): Promise<number> {
        let [formData, options] = ImageService.createRequestData(imgFile);

        return this.http.post(ImageService.apiUrl, formData, options)
            .map(ImageService.processImageIdFromResponse)
            .toPromise()
            .catch(ImageService.handleDeserialisationError);
    }

    /**
     * Update the given image ID to refer to the new given image file
     * @param imageId the existing image ID that should now point to the new image
     * @param imgFile the new image to upload
     * @return the ID of the image (not needed, but good to check the return)
     */
    public updateImage(imageId: number, imgFile: File): Promise<number> {
        let [formData, options] = ImageService.createRequestData(imgFile);

        return this.http.put(ImageService.apiUrl + imageId, formData, options)
            .map(ImageService.processImageIdFromResponse)
            .toPromise()
            .catch(ImageService.handleDeserialisationError);
    }

    /**
     * Delete the image with the given ID
     * @param imageId the ID of the image to delete
     */
    public deleteImage(imageId: number): Promise<any> {
        return this.http.delete(ImageService.apiUrl + imageId)
            .toPromise();
    }

    /**
     * Get a list of all image IDs from the backend
     * @return a promise of all the IDs of all images in the database
     */
    public listAllImages(): Promise<number[]> {
        return this.http.get(ImageService.apiUrl)
            .map(resp => {
                let [success, value] = ImageService.reviveResponseObject(resp.json());

                let ids = value as number[];
                if (!success || !ids) {
                    throw new Error(value.reason || 'Bad response from server');
                }

                return ids;
            })
            .toPromise()
            .catch(ImageService.handleDeserialisationError);
    }

    /**
     * Wrap a given file in a formdata object and supply request options to make
     * an HTTP request to upload the file with 'Content-Type': 'multipart/form-data' functionality
     * @param imgFile the image file to wrap
     */
    private static createRequestData(imgFile: File): [FormData, RequestOptions] {
        let formData: FormData = new FormData();
        formData.append(ImageService.formDataName, imgFile);

        let headers: Headers = new Headers();

        let options: RequestOptions = new RequestOptions({ headers: headers });

        return [formData, options];
    }

    /**
     * Take an arbitrary JSON object from the server and attempt to
     * turn it into a known response type
     * @param json the object received from the server
     */
    private static reviveResponseObject(json: any): [boolean, any] {
        if (json['status'] == 'failure' && json['reason']) {
            return [false, new ErrorResponse(json['reason'])];
        }

        if (json as number[]) {
            return [true, json as number[]];
        }

        if (json as number) {
            return [true, json as number];
        }

        return [false, new ErrorResponse('Bad response from server: ' + JSON.stringify(json))];
    }

    /**
     * Take an HTTP response and try to process a numeric ID from it
     * @param resp the received HTTP response object
     */
    private static processImageIdFromResponse(resp: Response): number {
        let [success, result] = ImageService.reviveResponseObject(resp.json());

        let id = result as number;
        if (!success || !id) {
            throw new Error(result.reason || 'Bad response from server');
        }

        return id;
    }

    /**
     * Handle errors that occur during deserialisation
     * @param error the error to handle
     */
    private static handleDeserialisationError(error: any): Promise<any> {
        console.error('An error occurred', error);
        return Promise.reject(error.message || error);
    }
}

/**
 * Class to represent error responses from the server
 * to be produced during deserialization
 */
export class ErrorResponse {
    public readonly reason: string;

    constructor(reason: string) {
        this.reason = reason;
    }
}
