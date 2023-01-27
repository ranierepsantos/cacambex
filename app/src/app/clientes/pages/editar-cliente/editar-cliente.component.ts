import { HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, OnInit, Optional, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MatTable } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { of } from 'rxjs';
import { ICEP } from 'src/app/identidade-acesso/interfaces/cep';
import { BuscadorCepService } from 'src/app/identidade-acesso/servicos/buscador-cep.service';

import { NovoCliente } from '../../interfaces/icliente';
import { NovoEndereco, VisualizarEndereco } from '../../interfaces/ienderecos';
import { ClienteService } from '../../servicos/cliente.service';
import { SnackResponseService } from './../../../design-system/snack-response.service';
import { AlterarCliente, VisualizarCliente } from './../../interfaces/icliente';
import { EditarEnderecoComponent } from './../editar-endereco/editar-endereco.component';

@Component({
  templateUrl: './editar-cliente.component.html',
  styleUrls: ['./editar-cliente.component.css'],
})
export class EditarClienteComponent implements OnInit {
  @ViewChild(MatTable) table!: MatTable<NovoEndereco>;
  displayedColumns: string[] = [
    'logradouro',
    'numero',
    'bairro',
    'cep',
    'cidade',
    'uf',
    'complemento',
    'acao',
  ];
  enviando!: boolean;
  enderecosDeEntrega: NovoEndereco[] = [];
  visualizarCliente = {} as VisualizarCliente;
  visualizarEndereco = {} as VisualizarEndereco;
  alterarCliente: AlterarCliente = {} as AlterarCliente;
  titulo = 'Novo cliente';
  clienteForm: FormGroup = {} as FormGroup;
  cliente: NovoCliente = {} as NovoCliente;
  endereco: NovoEndereco = {} as NovoEndereco;
  dataNasc = new Date;
  tipoDocCpf = 0;
  tipoDocCnpj = 1;
  dataNascimentoDesabilitada = false;
  editarCliente!: boolean;
  urlAtual;

  constructor(
    private snackBar: SnackResponseService,
    private dialog: MatDialog,
    private clienteService: ClienteService,
    private router: Router,
    private route: ActivatedRoute,
    @Optional() @Inject(MAT_DIALOG_DATA) public isDialog: any = false,
    private buscadorCepService: BuscadorCepService,
    public dialogRef: MatDialogRef<EditarClienteComponent>,
    private fb: FormBuilder
  ) {
    this.urlAtual = this.router.getCurrentNavigation()?.previousNavigation?.finalUrl?.toString();
  }
  ngOnInit(): void {
    const cliente = this.route.snapshot.data['cliente'];
    this.enderecosDeEntrega = cliente.enderecosEntrega;
    if (cliente.id) {
      this.urlAtual === '/clientes' ?
        this.titulo = 'Editar cliente'
        : this.titulo = 'Meus dados';
      this.editarCliente = true;
    }
    this.clienteForm = this.fb.group({
      id: [cliente.id],
      nome: [cliente.nome, [Validators.required, Validators.maxLength(255)]],
      documento: [cliente.documento, [Validators.required, Validators.minLength(11), Validators.maxLength(16)]],
      tipoDocumento: [cliente.tipoDocumento],
      contribuinte: ['S'],
      telefone: [cliente.telefone, [Validators.required, Validators.maxLength(16)]],
      email: [cliente.email, [Validators.required, Validators.email]],
      dataNascimento: [cliente.dataNascimento, [Validators.nullValidator]],
      enderecoCobranca: this.fb.group({
        id: [cliente.enderecoCobranca.id],
        cep: [cliente.enderecoCobranca.cep, [Validators.required, Validators.maxLength(9)]],
        logradouro: [cliente.enderecoCobranca.logradouro, [Validators.required, Validators.maxLength(255)]],
        numero: [cliente.enderecoCobranca.numero, [Validators.required, Validators.maxLength(50)]],
        bairro: [cliente.enderecoCobranca.bairro, [Validators.required, Validators.maxLength(255)]],
        cidade: [cliente.enderecoCobranca.cidade, [Validators.required, Validators.maxLength(255)]],
        complemento: [cliente.enderecoCobranca.complemento, [Validators.maxLength(255)]],
        uf: [cliente.enderecoCobranca.uf, [Validators.required, Validators.maxLength(11), Validators.pattern('[a-zA-Z]+')]]
      }),
      enderecosEntrega: this.fb.array([])
    });
  }

  submit() {
    this.enviando = true;
    this.setTipoDocumento();
    let id = this.clienteForm.get('id')?.value;

    if (id) {
      this.snackBar.mostrarMensagem("Salvando alterações, aguarde.")
      this.enviando = true;
      this.clienteService.alterarCliente(this.clienteForm.value).subscribe({
        next: () => {
          this.router.navigate(['/clientes']),
            this.snackBar.mostrarMensagem("As alterações foram salvas.")
        },
        error: (e) => {
          this.snackBar.mostrarMensagem(e.error.mensagem, true)
        }
      })
    } else {
      this.snackBar.mostrarMensagem("Registando novo cliente, aguarde.")
      this.enviando = true;
      this.clienteService.criarCliente(this.clienteForm.value).subscribe({
        next: () => {
          this.router.navigate(['/clientes']),
            this.snackBar.mostrarMensagem("Cliente registrado com sucesso!.")
        },
        error: (e) => {
          this.snackBar.mostrarMensagem(e.error.mensagem, true)
        }
      })
    }
  }

  novoEndereco() {
    const dialogRef = this.dialog.open(EditarEnderecoComponent);
    dialogRef.afterClosed().subscribe((endereco: NovoEndereco) => {
      if (endereco == undefined) return of();
      else this.enderecosDeEntrega.push(endereco);
      this.adicionarEnderecoEntrega(endereco);
      this.table.renderRows();
      this.snackBar.mostrarMensagem('Endereço criado com sucesso.');
      return endereco;
    });
  }
  alterarEndereco(endereco: VisualizarEndereco) {
    const dialofRef = this.dialog.open(EditarEnderecoComponent, {
      data: endereco,
    });
    dialofRef.afterClosed().subscribe((x) => {
      if (x == undefined) return of();
      this.snackBar.mostrarMensagem('Endereço alterado com sucesso.');
      return this.alterar(endereco, x);
    });
  }
  private alterar(endereco: VisualizarEndereco, x: any) {
    let index = this.enderecosDeEntrega.indexOf(endereco);
    this.enderecosEntrega2.at(index).patchValue({
      cep: x.cep,
      logradouro: x.logradouro,
      numero: x.numero,
      bairro: x.bairro,
      cidade: x.cidade,
      uf: x.uf,
      complemento: x.complemento
    });
    this.enderecosDeEntrega[index] = x;
    this.table.renderRows();
  }

  excluirEndereco(visualizarEndereco: VisualizarEndereco) {
    const index = this.enderecosDeEntrega.indexOf(visualizarEndereco);
    this.enderecosEntrega2.removeAt(index);
    this.enderecosDeEntrega.splice(index, 1);
    this.table.renderRows();
    this.snackBar.mostrarMensagem('Endereço excluído com sucesso.');
  }
  voltar() {
    let url;
    this.urlAtual === "/clientes" ?
      url = "/clientes" :
      url = "/pedidos";
    this.router.navigate([url]);

  }
  encontrarEndereco(cep: string) {
    this.buscadorCepService.getAddress(cep).subscribe((x: ICEP) => {
      this.visualizarEndereco.cep = x.cep;
      this.visualizarEndereco.logradouro = x.logradouro;
      this.visualizarEndereco.bairro = x.bairro;
      this.visualizarEndereco.cidade = x.localidade;
      this.visualizarEndereco.uf = x.uf;
      console.log(x);
    }, (e: HttpErrorResponse) => {
      this.snackBar.mostrarMensagem("Informe um CEP válido!", true);
    })
  }
  onEnderecoFormGroupChange(endereco: NovoEndereco) {
    this.clienteForm.patchValue({
      enderecoCobranca: {
        cep: endereco.cep,
        logradouro: endereco.logradouro,
        numero: endereco.numero,
        bairro: endereco.bairro,
        cidade: endereco.cidade,
        uf: endereco.uf,
        complemento: endereco.complemento
      }
    })
  }

  onClienteFormGroupChange(cliente: NovoCliente) {
    this.clienteForm.patchValue({
      nome: cliente.nome,
      documento: cliente.documento,
      tipoDocumento: cliente.tipoDocumento,
      telefone: cliente.telefone,
      email: cliente.email,
      dataNascimento: cliente.dataNascimento
    })
  }
  setTipoDocumento() {
    this.documento.value.length == 11 ? this.tipoDocumento.setValue(this.tipoDocCpf) : this.tipoDocumento.setValue(this.tipoDocCnpj);
  }

  adicionarEnderecoEntrega(endereco: NovoEndereco) {
    const endEntrega = this.fb.group({
      cep: [endereco.cep, [Validators.required, Validators.maxLength(9)]],
      logradouro: [endereco.logradouro, [Validators.required, Validators.maxLength(255)]],
      numero: [endereco.numero, [Validators.required, Validators.maxLength(50)]],
      bairro: [endereco.bairro, [Validators.required, Validators.maxLength(255)]],
      cidade: [endereco.cidade, [Validators.required, Validators.maxLength(255)]],
      complemento: [endereco.complemento, [Validators.maxLength(255)]],
      uf: [endereco.uf, [Validators.required, Validators.maxLength(11), Validators.pattern('[a-zA-Z]+')]],
    });
    this.enderecosEntrega2.push(endEntrega);
  }
  get documento() { return this.clienteForm.get('documento') as FormControl; }
  get tipoDocumento() { return this.clienteForm.get('tipoDocumento') as FormControl; }
  get enderecosEntrega2() { return this.clienteForm.controls["enderecosEntrega"] as FormArray; }
}
