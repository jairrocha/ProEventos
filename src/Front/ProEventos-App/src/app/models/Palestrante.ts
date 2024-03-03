import { Evento } from "./Evento";
import { RedeSocial } from "./RedeSocial";

export interface Palestrante {

  id:Number;
  nome:string;
  minicurriculo:string;
  imagemURL:string;
  telefone:string;
  email:string;
  redeSociais:RedeSocial[];
  palestranteEventos:Evento[];

}
