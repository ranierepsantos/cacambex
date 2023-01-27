import { registerLocaleData } from '@angular/common';
import localePt from '@angular/common/locales/pt';
import { Component, LOCALE_ID, OnInit } from '@angular/core';
import { FormBuilder, FormControl, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';

import { NovoCliente } from './../../../clientes/interfaces/icliente';
import { NovoEndereco } from './../../../clientes/interfaces/ienderecos';
import { AutoCadastroServico } from './../../servicos/auto-cadastro.service';

registerLocaleData(localePt);

@Component({
  templateUrl: './auto-cadastro.component.html',
  styleUrls: ['./auto-cadastro.component.css'],
  providers: [
    { provide: LOCALE_ID, useValue: 'pt-BR' }
  ]
})
export class AutoCadastroComponent implements OnInit {
  cliente: NovoCliente = {} as NovoCliente;
  endereco: NovoEndereco = {} as NovoEndereco;
  nomeBotao = 'Proximo';
  enviando!: boolean;
  autoCadastroForm: UntypedFormGroup = {} as UntypedFormGroup;
  hide = true;
  dataNasc = new Date;
  tipoDocCpf = 0;
  tipoDocCnpj = 1;
  dataNascimentoDesabilitada = false;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private autoCadastroServico: AutoCadastroServico,
    private snackBar: SnackResponseService,
  ) { }

  ngOnInit(): void {
    this.autoCadastroForm = this.fb.group({
      cliente: this.fb.group({
        nome: ['', [Validators.required, Validators.maxLength(255)]],
        documento: ['', [Validators.required, Validators.minLength(11), Validators.maxLength(11)]],
        tipoDocumento: [, Validators.nullValidator],
        contribuinte: [''],
        telefone: ['', [Validators.required, Validators.maxLength(16)]],
        email: ['', [Validators.required, Validators.email]],
        dataNascimento: [this.dataNasc.toISOString(), [Validators.nullValidator]],
        enderecoCobranca: this.fb.group({
          cep: ['', [Validators.required, Validators.maxLength(9)]],
          logradouro: ['', [Validators.required, Validators.maxLength(255)]],
          numero: ['', [Validators.required, Validators.maxLength(50)]],
          bairro: ['', [Validators.required, Validators.maxLength(255)]],
          cidade: ['', [Validators.required, Validators.maxLength(255)]],
          complemento: ['', [Validators.maxLength(255)]],
          uf: ['', [Validators.required, Validators.maxLength(11), Validators.pattern('[a-zA-Z]+')]]
        }),
        enderecosEntrega: this.fb.array([])
      }),
      senha: ['', [Validators.required, Validators.minLength(6)]],
      confirmarSenha: [''],
    });
  }
  submit() {
    this.enviando = true;
    this.setTipoDocumento();
    this.snackBar.mostrarMensagem("Aguarde enquanto realizamos o seu cadastro.");

    this.autoCadastroServico.criar(this.autoCadastroForm.value).subscribe((x: any) => {
      this.snackBar.mostrarMensagem("Cadastro realizado com sucesso.");
      this.router.navigate(['/']);
    }, (e: any) => {
      if (e.error.message) {
        this.snackBar.mostrarMensagem(e.error.message, true)
      }
      else {
        this.snackBar.mostrarMensagem("Ocorreu um erro. Tente novamente em dois minutos.", true)
      }
      this.enviando = false;
    });
  }
  voltar() {
    this.router.navigate(['/']);
  }
  setTipoDocumento() {
    this.documento.value.length == 11 ? this.tipoDocumento.setValue(0) : this.tipoDocumento.setValue(1);
  }
  onEnderecoFormGroupChange(endereco: NovoEndereco) {
    this.endereco.cep = endereco.cep;
    this.endereco.logradouro = endereco.logradouro;
    this.endereco.numero = endereco.numero;
    this.endereco.bairro = endereco.bairro;
    this.endereco.cidade = endereco.cidade;
    this.endereco.uf = endereco.uf;
    this.endereco.complemento = endereco.complemento;
    this.autoCadastroForm.patchValue({
      cliente: {
        enderecoCobranca: {
          cep: this.endereco.cep,
          logradouro: this.endereco.logradouro,
          numero: this.endereco.numero,
          bairro: this.endereco.bairro,
          cidade: this.endereco.cidade,
          uf: this.endereco.uf,
          complemento: this.endereco.complemento
        }
      }
    })
  }

  onClienteFormGroupChange(cliente: NovoCliente) {
    this.cliente.nome = cliente.nome;
    this.cliente.documento = cliente.documento;
    this.cliente.tipoDocumento = cliente.tipoDocumento;
    this.cliente.contribuinte = cliente.contribuinte;
    this.cliente.telefone = cliente.telefone;
    this.cliente.email = cliente.email;
    this.cliente.dataNascimento = cliente.dataNascimento;
    this.autoCadastroForm.patchValue({
      cliente: {
        nome: this.cliente.nome,
        documento: this.cliente.documento,
        tipoDocumento: this.cliente.tipoDocumento,
        contribuinte: this.cliente.contribuinte,
        telefone: this.cliente.telefone,
        email: this.cliente.email,
        dataNascimento: this.cliente.dataNascimento,
        endereco: {
          cep: this.endereco.cep,
          logradouro: this.endereco.logradouro,
          numero: this.endereco.numero,
          bairro: this.endereco.bairro,
          cidade: this.endereco.cidade,
          uf: this.endereco.uf,
          complemento: this.endereco.complemento
        }
      }
    })
  }
  get documento() { return this.autoCadastroForm.get('cliente.documento') as FormControl; }
  get tipoDocumento() { return this.autoCadastroForm.get('cliente.tipoDocumento') as FormControl; }
  get senha() { return this.autoCadastroForm.get('senha') as FormControl; }
  get confirmarSenha() { return this.autoCadastroForm.get('confirmarSenha') as FormControl; }

}
