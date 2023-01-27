import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UsuarioDecodificado } from 'src/app/identidade-acesso/interfaces/usuario-decodificado';
import { TokenServico } from 'src/app/identidade-acesso/servicos/token.servico';

@Component({
  selector: "ca-nav-menu",
  templateUrl: "./nav-menu.component.html",
  styleUrls: ["./nav-menu.component.css"],
})
export class NavMenuComponent implements OnInit {
  panelOpenState = false;
  public open: boolean = false;
  usuario: UsuarioDecodificado = {
    email: "",
    role: "",
    name: "",
    nameid: ""
  };
  usuarioId = "";
  constructor(
    private tokenServico: TokenServico,
    private router: Router

  ) { }

  ngOnInit(): void {
    this.tokenServico.usuario.subscribe((x) => {
      this.usuario = x
    }
    );
  }
  editarCliente() {
    this.router.navigate(['editar-cliente', this.usuario.nameid]);
  }
  alterarSenha() {
    this.router.navigateByUrl('redefinir-senha');
  }
}
