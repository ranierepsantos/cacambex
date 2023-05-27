import { AfterViewInit, ChangeDetectorRef, Component, Input, OnInit, ViewChild } from '@angular/core';
import { FormControl, NonNullableFormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatStepper } from '@angular/material/stepper';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { VisualizarCliente } from 'src/app/clientes/interfaces/icliente';
import { EditarEnderecoComponent } from 'src/app/clientes/pages/editar-endereco/editar-endereco.component';
import { ClienteService } from 'src/app/clientes/servicos/cliente.service';
import { SnackResponseService } from 'src/app/design-system/snack-response.service';
import { UsuarioDecodificado } from 'src/app/identidade-acesso/interfaces/usuario-decodificado';
import { TokenServico } from 'src/app/identidade-acesso/servicos/token.servico';

import { NovoEndereco, NovoEnderecoComClienteId, VisualizarEndereco } from './../../../clientes/interfaces/ienderecos';
import { PedidoService } from './../../servicos/pedido.service';
import { TipoCacambaServico } from 'src/app/tipo-cacamba/servicos/tipo-cacamba.service';
import { VisualizarTipoCacamba } from 'src/app/tipo-cacamba/interfaces/itipo-cacamba';


@Component({
  templateUrl: "./novo-pedido.component.html",
  styleUrls: ["./novo-pedido.component.css"],

})
export class NovoPedidoComponent implements OnInit, AfterViewInit {
  @ViewChild('stepper') private stepper!: MatStepper;
  enviando!: boolean;
  enderecosDoCliente!: VisualizarEndereco[];
  clienteId!: number;
  endId = 0;
  enderecoFiltrado = "";
  totalPedido = 0;
  valorPedido$!: Observable<number>;
  enderecoId = new FormControl<VisualizarEndereco | number>(0, Validators.required);
  usuarioDecodificado: UsuarioDecodificado = {} as UsuarioDecodificado;
  pedidoForm = this.fb.group({
    clienteId: [this.clienteId, [Validators.required]],
    volumeCacamba: [''],
    enderecoId: [this.endId],
    tipoDePagamento: [''],
    observacao: [''],
    valorPedido: ['']
  })
  listCacambas: VisualizarTipoCacamba[] = [];
  
  constructor(
    private clienteService: ClienteService,
    private fb: NonNullableFormBuilder,
    private snackBar: SnackResponseService,
    private router: Router,
    private dialog: MatDialog,
    private pedidoServico: PedidoService,
    private tokenService: TokenServico,
    private tipoCacambaService: TipoCacambaServico,
    private cdf: ChangeDetectorRef
  ) {
    this.tokenService.usuario.subscribe((x => {
      this.usuarioDecodificado = x;
    }));
  }
  ngAfterViewInit(): void {
    this.usuarioDecodificado.role == 'Cliente' ?
      this.clienteLogado()
      : null;
    setTimeout(() => {
    }, 100)
    this.cdf.detectChanges();
  }

  ngOnInit(): void {
    // const tipos$ = this.tipoCacambaService.obter(0,9999, "asc");
    // tipos$.subscribe((x) => x.data.forEach(x => this.listCacambas.push(x)));
  }

  filtrarEndereco() {
    let endereco = this.enderecosDoCliente.find((x) => x.id == (this.pedidoForm.get('enderecoId')?.value));
    this.enderecoFiltrado = `${endereco?.logradouro},
            ${endereco?.numero},
            ${endereco?.bairro},
            ${endereco?.cidade}`;
  }
  onClienteFormGroupChangeEvent(id: any) {
    const clienteId = parseInt(id);
    this.clienteService.ObterClientePorId(clienteId).subscribe((x: VisualizarCliente) => {
      this.enderecosDoCliente = x.enderecosEntrega;

      this.pedidoForm.patchValue({
        clienteId: x.id
      })
      this.pedidoForm.value.clienteId = x.id;
    })
  }
  private clienteLogado() {
    this.onClienteFormGroupChangeEvent(this.usuarioDecodificado.nameid);
    this.cliente.setValidators([Validators.nullValidator]);
    this.stepper.next();
  }
  onSubmit() {
    if (this.enderecoId.value == 0) {
      this.snackBar.mostrarMensagem("Necessário selecionar um endereço.")
    }
    else {
      this.snackBar.mostrarMensagem("Registrando seu pedido, aguarde.")
      this.enviando = true;
      this.pedidoServico.criarPedido(this.pedidoForm.value).subscribe((result: any) => {
        this.router.navigate(['/pedidos']);
        this.snackBar.mostrarMensagem("Pedido registrado com sucesso!");
      }, (err: any) => {
        this.enviando = false;
        this.snackBar.mostrarMensagem(err.error.errosOmie.errosOmie, true)
      })
      this.enviando = false;
    }
  }

  novoEndereco() {
    const dialogRef = this.dialog.open(EditarEnderecoComponent, {

    })
    dialogRef.afterClosed().subscribe((endereco) => {
      if (endereco == undefined) return of()
      this.enderecosDoCliente.push(endereco);
      return this.criarEndereco(endereco, this.pedidoForm.value.clienteId);
    })
  }
  private criarEndereco(endereco: NovoEndereco, clienteId: any) {

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
  async onSelectEnderecoChange(endereco: any) {
    this.pedidoForm.get('enderecoId')?.patchValue(endereco.value.id);
    this.carregarListaTipoCacambas(endereco.value.cep, this.pedidoForm.get('volumeCacamba')?.value)
  }

  definirPrecoPedido(preco: number) {
    this.pedidoForm.get('valorPedido')?.setValue(preco.toFixed(2));
    this.enderecoId.enable();
  }

  async carregarListaTipoCacambas(cep: string, volume: string = "") {
    this.listCacambas = [];
    const tipos$ = await this.tipoCacambaService.listarComPrecoFaixaCep(cep);
    tipos$.subscribe((x) => x.forEach(x => {
      if (x.precoFaixaCep.length > 0) x.preco = x.precoFaixaCep[0].preco
      this.listCacambas.push(x)
      if (x.volume == volume) {
        this.definirPrecoPedido(x.preco)
      }
    }));
  }

  get cliente() { return this.pedidoForm.get('clienteId') as FormControl; }
  get observacao() { return this.pedidoForm.get('observacao') as FormControl; }
  get endereco() { return this.pedidoForm.get('enderecoId') as FormControl; }
}

