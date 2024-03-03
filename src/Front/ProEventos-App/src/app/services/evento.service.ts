import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';
import {take} from 'rxjs/operators';

@Injectable()
export class EventoService {
  baseURL = 'https://127.0.0.1:5001/api/Evento'
  constructor(private http: HttpClient) { }

  public getEvento():Observable<Evento[]> {
    return this.http.get<Evento[]>(this.baseURL).pipe(take(1));
  }

  public getEventosByTema(tema:string):Observable<Evento[]> {
    return this.http.get<Evento[]>(`${this.baseURL}/tema/${tema}`).pipe(take(1));;
  }

  public getEventoById(id:number):Observable<Evento> {
    return this.http.get<Evento>(`${this.baseURL}/${id}`).pipe(take(1));;
  }

  public post (evento:Evento) : Observable<Evento>{
    return this.http.post<Evento>(this.baseURL, evento)
  }

  public put(evento: Evento):Observable<Evento> {
     return this.http.put<Evento>(`${this.baseURL}/${evento.id}`, evento).pipe(take(1));;
  }

  public deleteEvento(id:number):Observable<string> {
    return this.http.delete<any>(`${this.baseURL}/${id}`).pipe(take(1));;
  }


}

