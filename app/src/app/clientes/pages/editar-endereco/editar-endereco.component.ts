import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { VisualizarEndereco } from 'src/app/clientes/interfaces/ienderecos';

import { AlterarEndereco, NovoEndereco } from '../../interfaces/ienderecos';

@Component({
  templateUrl: './editar-endereco.component.html',
  styleUrls: ['./editar-endereco.component.css']
})
export class EditarEnderecoComponent implements OnInit {
  enderecoForm: FormGroup = {} as FormGroup;
  titulo = 'Novo endereço';
  alterarEndereco: AlterarEndereco = {} as AlterarEndereco;
  novoEndereco: NovoEndereco = {} as NovoEndereco;
  endereco = {} as VisualizarEndereco;

  constructor(
    @Inject(MAT_DIALOG_DATA) private x: VisualizarEndereco,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    if (this.x) {
      this.titulo = "Editar endereço";
      this.enderecoForm = this.fb.group({
        cep: [this.x.cep, [Validators.required, Validators.maxLength(9)]],
        logradouro: [this.x.logradouro, [Validators.required, Validators.maxLength(255)]],
        numero: [this.x.numero, [Validators.required, Validators.maxLength(50)]],
        bairro: [this.x.bairro, [Validators.required, Validators.maxLength(255)]],
        cidade: [this.x.cidade, [Validators.required, Validators.maxLength(255)]],
        complemento: [this.x.complemento, [Validators.maxLength(255)]],
        uf: [this.x.uf, [Validators.required, Validators.maxLength(11), Validators.pattern('[a-zA-Z]+')]],
      })
    }
    else {
      this.enderecoForm = this.fb.group({
        cep: ['', [Validators.required, Validators.maxLength(9)]],
        logradouro: ['', [Validators.required, Validators.maxLength(255)]],
        numero: ['', [Validators.required, Validators.maxLength(50)]],
        bairro: ['', [Validators.required, Validators.maxLength(255)]],
        cidade: ['', [Validators.required, Validators.maxLength(255)]],
        complemento: ['', [Validators.maxLength(255)]],
        uf: ['', [Validators.required, Validators.maxLength(11), Validators.pattern('[a-zA-Z]+')]],
        nomeContato: ['', [Validators.maxLength(255)]],
        observacao: ['', [Validators.maxLength(255)]],
      })
    }
  }
  onEnderecoFormGroupChange(endereco: NovoEndereco) {
    this.endereco.cep = endereco.cep;
    this.endereco.logradouro = endereco.logradouro;
    this.endereco.numero = endereco.numero;
    this.endereco.bairro = endereco.bairro;
    this.endereco.cidade = endereco.cidade;
    this.endereco.uf = endereco.uf;
    this.endereco.complemento = endereco.complemento;

    this.enderecoForm.patchValue({
      cep: this.endereco.cep,
      logradouro: this.endereco.logradouro,
      numero: this.endereco.numero,
      bairro: this.endereco.bairro,
      cidade: this.endereco.cidade,
      uf: this.endereco.uf,
      complemento: this.endereco.complemento
    })
  }

  data() {
    if (this.x) {
      this.alterarEndereco.id = this.x.id;
      this.alterarEndereco.cep = this.endereco.cep;
      this.alterarEndereco.logradouro = this.endereco.logradouro;
      this.alterarEndereco.numero = this.endereco.numero;
      this.alterarEndereco.bairro = this.endereco.bairro;
      this.alterarEndereco.complemento = this.endereco.complemento;
      this.alterarEndereco.cidade = this.endereco.cidade;
      this.alterarEndereco.uf = this.endereco.uf;
      return this.alterarEndereco;
    }
    this.novoEndereco.cep = this.endereco.cep;
    this.novoEndereco.logradouro = this.endereco.logradouro;
    this.novoEndereco.numero = this.endereco.numero;
    this.novoEndereco.complemento = this.endereco.complemento;
    this.novoEndereco.bairro = this.endereco.bairro;
    this.novoEndereco.cidade = this.endereco.cidade;
    this.novoEndereco.uf = this.endereco.uf;
    return this.novoEndereco;
  }
}
