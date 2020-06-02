import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';

@Injectable()
export class ValueService
{
  private baseUrl: string = '/api'

  constructor(private http: Http)
  {

  }

  getAll(): Observable<string[]>
  {
    let values$ = this.http
      .get(`${this.baseUrl}/values/`, { headers: this.getHeaders() })
      .map(res => res.json())
      .catch(handleError);

    return values$;
  }

  private getHeaders()
  {
    let headers = new Headers();
    headers.append('Accept', 'application/json');
    return headers;
  }
}

function mapValues(response: Response): string[]
{
  return response.json().results.map(toValue);
}

function toValue(r: any): string
{
  console.log(r);
  return JSON.stringify(r);
}

function handleError (error: any) {
  // log error
  // could be something more sophisticated
  let errorMsg = error.message || `Yikes! There was was a problem with our hyperdrive device and we couldn't retrieve your data!`
  console.error(errorMsg);

  // throw an application level error
  return Observable.throw(errorMsg);
}
