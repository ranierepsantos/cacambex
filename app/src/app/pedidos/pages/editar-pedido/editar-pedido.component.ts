import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { VisualizarCacamba } from 'src/app/cacambas/interfaces/icacamba';
import { CacambaServico } from 'src/app/cacambas/servicos/cacamba.service';
import { NovoEndereco, NovoEnderecoComClienteId, VisualizarEndereco } from 'src/app/clientes/interfaces/ienderecos';
import { EditarEnderecoComponent } from 'src/app/clientes/pages/editar-endereco/editar-endereco.component';
import { ClienteService } from 'src/app/clientes/servicos/cliente.service';
import { Paginacao } from 'src/app/identidade-acesso/interfaces/paginacao';

import { VisualizarPedido } from '../../interfaces/ipedido';
import { SnackResponseService } from './../../../design-system/snack-response.service';
import { PedidoService } from './../../servicos/pedido.service';

@Component({
  templateUrl: './editar-pedido.component.html',
  styleUrls: ['./editar-pedido.component.css'],
})
export class EditarPedidoComponent implements OnInit {
  enderecoId = new FormControl<VisualizarEndereco | null>(null, Validators.required);
  enderecoFiltrado = "";
  enviando!: boolean;
  enderecosDoCliente!: VisualizarEndereco[];
  valorPedido = 0;
  pedidoId!: number;
  clienteId!: number;
  cacamba3m$!: Observable<VisualizarCacamba[]>;
  cacamba5m$!: Observable<VisualizarCacamba[]>;
  visualizarPedido: VisualizarPedido = {} as VisualizarPedido;
  CacambaDataSource: Paginacao<VisualizarCacamba> = {} as Paginacao<VisualizarCacamba>;

  editarPedidoForm: FormGroup = {} as FormGroup;
  constructor(
    private snackBar: SnackResponseService,
    private router: Router,
    private dialog: MatDialog,
    private route: ActivatedRoute,
    private pedidoService: PedidoService,
    private fb: FormBuilder,
    private cacambaService: CacambaServico,
    private clienteService: ClienteService
  ) { }

  ngOnInit(): void {
    const pedido = this.route.snapshot.data['pedido'];
    this.clienteId = pedido.idCliente;
    this.valorPedido = pedido.valorPedido;
    this.visualizarPedido = pedido;

    this.clienteService.ObterClientePorId(pedido.idCliente).subscribe((x) => {
      this.enderecosDoCliente = x.enderecosEntrega;
      this.filtrarEndereco();
    })

    this.editarPedidoForm = this.fb.group({
      pedidoId: [pedido.id],
      enderecoId: [pedido.enderecoEntrega.id],
      volumeCacamba: [pedido.pedidoItem.volumeCacamba],
      tipoDePagamento: [pedido.tipoDePagamento],
      observacao: [pedido.observacao]
    })

    const cacambas$ = this.cacambaService.obterApenasCacambas();

    this.cacamba3m$ = cacambas$.pipe(
      map((x: any) => {
        return [x.filter((cacamba: any) => cacamba.volume == "3M³")[0]]
      })
    );

    this.cacamba5m$ = cacambas$.pipe(
      map((x: any) => {
        return [x.filter((cacamba: any) => cacamba.volume == "5M³")[0]]
      })
    );
  }
  onSubmit() {
    this.snackBar.mostrarMensagem("Processando..")
    this.enviando = true;
    this.pedidoService.alterarPedido(this.editarPedidoForm.value).subscribe(() => {
      this.snackBar.mostrarMensagem("Pedido alterado com sucesso!")
      this.router.navigate(['/pedidos']);
      this.enviando = false;
    }, (e: any) => {
      this.enviando = false;
      this.snackBar.mostrarMensagem(e.error.mensagem, true)
    })
  }

  filtrarEndereco() {
    let endereco = this.enderecosDoCliente.find((x) => x.id == (this.editarPedidoForm.get('enderecoId')?.value));
    this.enderecoFiltrado = `${endereco?.logradouro},
            ${endereco?.numero},
            ${endereco?.bairro},
            ${endereco?.cidade}`;
  }

  novoEndereco() {
    const dialogRef = this.dialog.open(EditarEnderecoComponent, {

    })
    dialogRef.afterClosed().subscribe((endereco) => {
      if (endereco == undefined) return of()
      this.enderecosDoCliente.push(endereco);
      return this.criarEndereco(endereco, this.clienteId);
    })
  }
  private criarEndereco(endereco: NovoEndereco, clienteId: number) {
    let novoEndereco = {} as NovoEnderecoComClienteId;
    novoEndereco.cep = endereco.cep;
    novoEndereco.logradouro = endereco.logradouro;
    novoEndereco.numero = endereco.numero;
    novoEndereco.complemento = endereco.complemento;
    novoEndereco.bairro = endereco.bairro;
    novoEndereco.cidade = endereco.cidade;
    novoEndereco.uf = endereco.uf;
    novoEndereco.clienteId = clienteId;

    this.clienteService.novoEndereco(novoEndereco).subscribe((x) => {
      this.snackBar.mostrarMensagem('Endereço cadastrado com sucesso!');
      window.location.reload();
    }, (e) => {
      this.snackBar.mostrarMensagem(e.error.message, true)
    })
  }
  definirPrecoPedido(preco: number) {
    this.valorPedido = preco;
    this.enderecoId.enable();
  }
  onSelectEnderecoChange(endereco: any) {
    this.editarPedidoForm.get('enderecoId')?.patchValue(endereco.value.id);
  }

  get observacao() { return this.editarPedidoForm.get('observacao') as FormControl; }
}
