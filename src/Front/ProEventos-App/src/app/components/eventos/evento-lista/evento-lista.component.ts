import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '@app/services/evento.service';
import { Evento } from '@app/models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { environment } from '@environments/environment';


@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef?: BsModalRef;
  public eventoId = 0;
  public eventos: Evento[] = [];
  public eventosFiltrados: Evento[] = [];
  public widthImg = 150;
  public marginImg = 2
  public showImg = true;
  private filtroListado = '';


  constructor(private eventoService: EventoService, private modalService: BsModalService,
      private toastr: ToastrService, private spinner: NgxSpinnerService, private router: Router) { }

  public ngOnInit(): void {
    this.spinner.show();
    this.CarregarEventos();
  }

  public CarregarEventos(): void {

    this.eventoService.getEvento().subscribe(
      {
        next: (eventos: Evento[]) => {
          this.eventos = eventos;
          this.eventosFiltrados = this.eventos;
        },
        error: (Error: any) => {
            this.spinner.hide()
            this.toastr.error('Erro ao carregar os eventos','Erro!')
          },
        complete: () => this.spinner.hide()
      }
    );

  }

  public alterarImg(): void {
    this.showImg = !this.showImg;
  }

  public mostrarImagem(imagemURL:string): string{
    return (imagemURL !== '')
      ? `${environment.apiUrl}Resources/Images/${imagemURL}`
      : `assets/img/semImagem.jpeg`
  }

  public get filtroLista(): string {
    return this.filtroListado;
  }

  public set filtroLista(value: string) {
    this.filtroListado = value;
    this.eventosFiltrados = this.filtroLista ? this.fitrarEventos(this.filtroLista) : this.eventos;
  }


  private fitrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLowerCase().indexOf(filtrarPor) !== -1 ||
        evento.local.toLowerCase().indexOf(filtrarPor) !== -1
    );
  }


  openModal(event:Event, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
   this.modalRef?.hide();
   this.spinner.show();
   this.eventoService.deleteEvento(this.eventoId).subscribe({
    next: (result: any) => {

        this.toastr.success('O evento foi deletado com sucesso.', 'Deletado!');
        this.spinner.hide();
        this.CarregarEventos();

    },
    error: (error: any) => {
      console.error(error);
      this.toastr.error(`Erro ao tentar deletar o evento. ${this.eventoId}`, 'Erro' )
      this.spinner.hide();
    },
    complete: () => this.spinner.hide()
   });


  }

  decline(): void {
    this.modalRef?.hide();
  }


  detalheEvento(id:number):void{
    this.router.navigate([`/eventos/detalhe/${id}`]);
  }

}
