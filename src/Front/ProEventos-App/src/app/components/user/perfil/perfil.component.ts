import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorFiled } from '@app/helpers/ValidatorFiled';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  form!:FormGroup;

  constructor(public fb: FormBuilder) { }


  ngOnInit(): void {
    this.Validation();
  }

  private Validation():void {

    const formOptions: AbstractControlOptions = {
      validators: ValidatorFiled.MustMatch('senha','confirmeSenha')
    };

    this.form = this.fb.group({
      titulo:['',Validators.required] ,
      primeiroNome:['',Validators.required] ,
      ultimoNome:['',Validators.required] ,
      email:['',[Validators.required, Validators.email]] ,
      telefone:['',[Validators.required]],
      funcao:['',[Validators.required]],
      descricao:['',Validators.required],
      senha:['',[Validators.required, Validators.minLength(6)]] ,
      confirmeSenha:['',Validators.required],
    }, formOptions)
  }

  get f():any{
    return this.form.controls;
  }

  public resetForm(event: Event):void{
    event.preventDefault();
    this.form.reset();
  }

}
