import { Funcao } from './../../enum/funcao';
import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { AlterarUsuario, NovoUsuario, VisualizarUsuario } from '../../interfaces/iusuario';


@Component({
  templateUrl: './editar-usuario.component.html',
  styleUrls: ['./editar-usuario.component.css']
})
export class EditarUsuarioComponent implements OnInit {
  novoUsuario: NovoUsuario = {} as NovoUsuario;
  alterarUsuario: AlterarUsuario = {} as AlterarUsuario;
  usuario = {} as VisualizarUsuario;
  titulo: string = "Novo usuário";

  constructor(@Inject(MAT_DIALOG_DATA) public x: VisualizarUsuario) { }

  ngOnInit(): void {
    if (this.x) {
      this.titulo = "Editar usuário";
      this.usuario.nome = this.x.nome;
      this.usuario.email = this.x.email;
      this.usuario.ativo = this.x.ativo;
      this.usuario.funcao = this.x.funcao;
    }
  }

  data() {
    if (this.x) {
      this.alterarUsuario.id = this.x.id;
      this.alterarUsuario.nome = this.usuario.nome;
      this.alterarUsuario.email = this.usuario.email;
      this.alterarUsuario.ativo = this.usuario.ativo;
      this.alterarUsuario.funcao = this.usuario.funcao;
      return this.alterarUsuario;
    }

    this.novoUsuario.nome = this.usuario.nome;
    this.novoUsuario.email = this.usuario.email;
    this.novoUsuario.funcao = this.usuario.funcao;
    return this.novoUsuario;
  }

}
