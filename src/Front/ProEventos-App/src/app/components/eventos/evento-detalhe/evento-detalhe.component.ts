import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { LoteService } from '@app/services/Lote.service';
import { EventoService } from '@app/services/evento.service';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';



@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  modalRef: BsModalRef
  eventoId: number;
  estadoSalvar: string = 'post';
  evento = {} as Evento;
  form: FormGroup = new FormGroup({});
  loteAtual = { id: 0, nome: '', indice: 0 };

  constructor(private fb: FormBuilder,
    private localeService: BsLocaleService,
    private activatedRoute: ActivatedRoute,
    private eventoService: EventoService,
    private spinner: NgxSpinnerService,
    private toaster: ToastrService,
    private router: Router,
    private modalService: BsModalService,
    private loteService: LoteService) {
    this.localeService.use('pt-br');

  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray
  }

  get modoEditar(): boolean {
    return this.estadoSalvar === 'put';
  }

  get f(): any {
    return this.form.controls;
  }

  public validation(): void {
    this.form = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(50)]],
      dataEvento: ['', [Validators.required]],
      qtdPessoas: ['', [Validators.required, Validators.minLength(1), Validators.maxLength(150)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      imagemURL: ['', Validators.required],
      lotes: this.fb.array([])
    });
  }

  adicionarLote(): void {
    this.lotes.push(this.criarLote({ id: 0 } as Lote));
  }

  criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],

      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim]
    });
  }


  CarregarEvento(): void {
    this.eventoId = +this.activatedRoute.snapshot.paramMap.get('id')

    if (this.eventoId !== null && this.eventoId !== 0) {
      this.spinner.show();

      this.estadoSalvar = 'put'

      this.eventoService.getEventoById(this.eventoId).subscribe({
        next: (evento: Evento) => {


          this.evento = { ...evento, dataEvento: new Date(evento.dataEvento) }; //spread operator serve para fazer cópia.
          this.form.patchValue(this.evento);
          this.evento.lotes.forEach(lote => {
            this.lotes.push(this.criarLote(lote))
          });
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toaster.error('Erro ao tentar carregar evento.');
        },
        complete: () => {
          this.spinner.hide();
        }
      });
    }


  }

  ngOnInit() {
    this.CarregarEvento();
    this.validation();
  }

  public resetForm(): void {
    this.form.reset();
  }

  get bsConfig(): any {
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY HH:mm',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  public cssValidator(campoForm: FormControl | AbstractControl): any {
    return { 'is-invalid': campoForm.errors && campoForm.touched }
  }

  public salvarEvento(): void {


    if (this.form.valid) {

      this.spinner.show();

      const selectedDate = new Date(this.form.value.dataEvento);
      const localeDate = selectedDate.toLocaleString('en-US', { timeZone: 'America/Sao_Paulo' });

      this.evento = (this.estadoSalvar === 'post')
        ? { ...this.form.value, dataEvento: localeDate }
        : { id: this.evento.id, ...this.form.value, dataEvento: localeDate };


      //strict deve ser falso no tsconfing.json para funcionar
      this.eventoService[this.estadoSalvar](this.evento).subscribe({
        next: (eventoRetorno: Evento) => {
          this.toaster.success("Evento salvo com sucesso!", 'Sucesso');
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`]);
        },
        error: (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toaster.error("Erro ao salvar o evento!", 'Erro')
        },
        complete: () => {
          this.spinner.hide()
        }
      });

    }
  }

  public mudarValorData(value: Date, indice:number, campo:string): void{
    this.lotes.value[indice][campo] = value;
  }

  public salvarLotes(): void {
    if (this.form.controls.lotes.valid) {

      this.spinner.show();
      let lotes = this.form.value.lotes;
      lotes = lotes.map(obj => ({ ...obj, eventoid: this.eventoId,
                dataInicio: obj.dataInicio.toLocaleString('pt-BR'),
                dataFim: obj.dataFim.toLocaleString('pt-BR')}));



      this.loteService.saveLote(this.eventoId, lotes).subscribe({
        next: () => {
          this.toaster.success('Lote(s) salvo(s) com sucesso!', 'Sucesso!');
        },
        error: (error: any) => {
          console.error(error);
          this.spinner.hide();
          this.toaster.error('Erro ao salvar tentar o(s) lote(s)!', 'Erro');
        },
        complete: () => {
          this.spinner.hide();
        }
      });
    }
  }

  public removerLote(template: TemplateRef<any>, indice: number): void {
    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });

  }

  public confirmDeleteLote(): void {

    this.modalRef.hide();
    this.spinner.show();

    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe({

      next: () => {
        this.toaster.success('Lote deletado com sucesso!', 'Sucesso');
        this.lotes.removeAt(this.loteAtual.indice);
      },
      error: (error: any) => {
        this.toaster.error(`Erro ao tentar deletar o Lote: ${this.loteAtual.nome}`);
      }
    }).add(() => this.spinner.hide());

  }

  public declineDeleteLote(): void {
    this.modalRef.hide();

  }

  public retornaTituloLote(lote: Lote): string{


    return lote.nome === null || lote.nome === ''
      ? 'Nome do lote'
      : lote.nome
  }

}




