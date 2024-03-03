import { Component, OnInit } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ValidatorFiled } from '@app/helpers/ValidatorFiled';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

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
      primeiroNome:['',Validators.required] ,
      ultimoNome:['',Validators.required] ,
      email:['',[Validators.required, Validators.email]] ,
      nomeUsuario:['',Validators.required] ,
      senha:['',[Validators.required, Validators.minLength(6)]] ,
      confirmeSenha:['',Validators.required]
    }, formOptions)
  }

  get f():any{
    return this.form.controls;
  }


}
