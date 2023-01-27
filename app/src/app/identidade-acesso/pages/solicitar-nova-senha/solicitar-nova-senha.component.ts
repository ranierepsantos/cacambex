import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';

import { RecuperarSenha } from '../../interfaces/autorizacao-usuario';
import { AutorizacaoServico } from '../../servicos/autorizacao.service';

@Component({
  templateUrl: './solicitar-nova-senha.component.html',
  styleUrls: ['./solicitar-nova-senha.component.css'],
})
export class SolicitarNovaSenhaComponent implements OnInit {
  recuperarSenhaForm: FormGroup = {} as FormGroup;
  recuperar: RecuperarSenha = {} as RecuperarSenha;
  enviando = false;
  constructor(
    private autorizacaoServico: AutorizacaoServico,
    private router: Router,
    private snackBar: SnackResponseService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.recuperarSenhaForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]]
    });
  }

  submit() {
    this.enviando = true;
    this.autorizacaoServico.recuperarSenha(this.recuperarSenhaForm.value).subscribe(
      () => {
        this.enviando = false;
        this.router.navigateByUrl('/identidade-acesso/confirmacao-email');
      },
      (e) => {
        this.enviando = false;
        this.snackBar.mostrarMensagem(e.error.mensagem, true);
      }
    );
  }
  get email() { return this.recuperarSenhaForm.get('email') as FormControl; }
}
