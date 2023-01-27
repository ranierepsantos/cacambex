import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { UsuarioDecodificado } from 'src/app/identidade-acesso/interfaces/usuario-decodificado';
import { AutorizacaoServico } from 'src/app/identidade-acesso/servicos/autorizacao.service';
import { TokenServico } from 'src/app/identidade-acesso/servicos/token.servico';

@Component({
  selector: "ca-nav",
  templateUrl: "./nav.component.html",
  styleUrls: ["./nav.component.css"],
})
export class NavComponent implements OnInit {
  @Input() showSearch: boolean = true;
  @Output() filter: EventEmitter<any> = new EventEmitter();
  @Input() subscription: boolean = false;
  @Input() signatoryName: string = "";
  @Output() toggleMenu: EventEmitter<void> = new EventEmitter();
  @Output() editarMeusDados: EventEmitter<void> = new EventEmitter();
  searchFocus: boolean = false;
  searchValues: string = " ";
  isOpenAdvanceFilter: boolean = false;
  usuario: UsuarioDecodificado = {
    email: "",
    role: "",
    name: "",
    nameid: ""
  };
  constructor(
    private autorizacaoServico: AutorizacaoServico,
    private tokenServico: TokenServico,
  ) { }

  ngOnInit(): void {
    this.tokenServico.usuario.subscribe((x) => {
      this.usuario = x
    });
  }

  sair() {
    this.autorizacaoServico.logout();
  }
}
