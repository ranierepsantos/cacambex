import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { UsuarioDecodificado } from 'src/app/identidade-acesso/interfaces/usuario-decodificado';
import { AutorizacaoServico } from 'src/app/identidade-acesso/servicos/autorizacao.service';
import { TokenServico } from 'src/app/identidade-acesso/servicos/token.servico';
import { VisualizarPedido } from 'src/app/pedidos/interfaces/ipedido';
import { PedidoService } from 'src/app/pedidos/servicos/pedido.service';

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
  notificacaoQuantidade: number = 0;
  notificacaoPedido: VisualizarPedido[] = [];
  usuario: UsuarioDecodificado = {
    email: "",
    role: "",
    name: "",
    nameid: ""
  };
  constructor(
    private autorizacaoServico: AutorizacaoServico,
    private tokenServico: TokenServico,
    private pedidoServico: PedidoService,
    private router: Router,
  ) { }

  ngOnInit(): void {
    this.tokenServico.usuario.subscribe((x) => {
      this.usuario = x
    });
    this.getNotificacoes();
  }

  sair() {
    this.autorizacaoServico.logout();
  }

  getNotificacoes() {
    this.pedidoServico.obterNotificacao(8).subscribe(data => {
      this.notificacaoQuantidade = data.length;
      this.notificacaoPedido = data;
    });
  }

  goToPedido(pedidoId: number) {
    this.router.navigate(["pedidos/gerenciar-pedido", pedidoId])
  }
}
