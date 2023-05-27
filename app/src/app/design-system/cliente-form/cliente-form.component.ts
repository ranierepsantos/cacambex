import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

import { NovoCliente } from './../../clientes/interfaces/icliente';

@Component({
  selector: 'ca-cliente-form',
  templateUrl: './cliente-form.component.html',
  styleUrls: ['./cliente-form.component.css']
})
export class ClienteFormComponent implements OnInit {
  @Input() editarCliente = false;
  @Input() clienteForm!: NovoCliente;
  @Output() onClienteFormGroupChange = new EventEmitter();
  cliente: FormGroup = {} as FormGroup;
  hide = true;
  dataNasc = new Date(1990, 1, 1);
  dataNascimentoDesabilitada = false;
  idade = 0
  mostrarCampoContribuinte = false;

  constructor(
    private fb: FormBuilder
  ) {
  }

  ngOnInit(): void {
    this.cliente = this.fb.group({
      nome: [this.clienteForm.nome, [Validators.required, Validators.maxLength(255)]],
      documento: [this.clienteForm.documento],
      tipoDocumento: [, Validators.nullValidator],
      contribuinte: ['S'],
      telefone: [this.clienteForm.telefone, [Validators.required, Validators.maxLength(16)]],
      email: [this.clienteForm.email, [Validators.required, Validators.email]],
      dataNascimento: [this.clienteForm.dataNascimento, [this.validaNascimento()]]

    })
    this.cliente.valueChanges.subscribe(() => this.onClienteFormGroupChange.emit(this.cliente.value));
  }

  setTipoDocumento() {
    this.documento.value.length == 11 ? this.tipoDocumento.setValue(0) : this.tipoDocumento.setValue(1);
  }
  
  mudarValidatorDocumento(documento: any) {
    let length = documento.value.length > 11 ? 14 : 11;
    this.cliente.controls["documento"].setValidators([Validators.minLength(length), Validators.maxLength(length)]);
  }

  validaNascimento(): ValidatorFn {
    return (control: AbstractControl): { [key: string]: boolean } | null => {
      if (this.verificarAnoNascimento(control.value) < 18) {
        return { 'menorIdade': true }
      }
      return null
    }
  }

  verificarAnoNascimento(dataNascimento: any) {
    var data = dataNascimento.value ? dataNascimento.value?.format('YYY-MM-DD') : dataNascimento;
    var hoje = new Date();
    var nascimento = new Date(data);

    this.idade = hoje.getFullYear() - nascimento.getFullYear();
    return this.idade;
  }
  desabilitarDocumento() {
    if (this.editarCliente == true) {
      this.documento.disable();
    }
  }
  get nome() { return this.cliente.get('nome') as FormControl; }
  get documento() { return this.cliente.get('documento') as FormControl; }
  get tipoDocumento() { return this.cliente.get('tipoDocumento') as FormControl; }
  get telefone() { return this.cliente.get('telefone') as FormControl; }
  get email() { return this.cliente.get('email') as FormControl; }
  get senha() { return this.cliente.get('senha') as FormControl; }
  get dataNascimento() { return this.cliente.get('dataNascimento') as FormControl; }
}
