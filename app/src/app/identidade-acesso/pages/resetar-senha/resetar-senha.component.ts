import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';
import { TokenServico } from 'src/app/identidade-acesso/servicos/token.servico';

import { AutorizacaoServico } from '../../servicos/autorizacao.service';

@Component({
  templateUrl: './resetar-senha.component.html',
  styleUrls: ['./resetar-senha.component.css'],
})
export class ResetarSenhaComponent implements OnInit {
  hide = true;
  token: string = ' ';
  enviando: boolean = false;

  mudarSenhaForm: FormGroup = {} as FormGroup;
  constructor(
    private router: Router,
    route: ActivatedRoute,
    private autorizacaoServico: AutorizacaoServico,
    private snackBar: SnackResponseService,
    private tokenServico: TokenServico,
    private fb: FormBuilder
  ) {
    route.paramMap.subscribe((x: any) => {
      this.token = x.params.token;
      this.tokenServico.removeToken();
    });
  }

  ngOnInit(): void {
    this.mudarSenhaForm = this.fb.group({
      novaSenha: ['', [Validators.required, Validators.minLength(6)]],
      confirmarNovaSenha: [''],
    })
  }

  submit() {
    this.enviando = true;
    this.autorizacaoServico
      .alterarSenha(this.mudarSenhaForm.value, this.token)
      .subscribe(
        (x) => {
          this.enviando = false;
          this.router.navigate(['/']);
          this.snackBar.mostrarMensagem(x.mensagem);
        },
        (e) => {
          this.enviando = false;
          e.error.dados.forEach((mensagem: any) => {
            this.snackBar.mostrarMensagem(mensagem, true);
          });
        }
      );
  }
  get novaSenha() { return this.mudarSenhaForm.get('novaSenha') as FormControl; }
  get confirmarNovaSenha() { return this.mudarSenhaForm.get('confirmarNovaSenha') as FormControl; }

}
