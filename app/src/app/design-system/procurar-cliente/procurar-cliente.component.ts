import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { map, startWith } from 'rxjs/operators';
import { VisualizarCliente } from 'src/app/clientes/interfaces/icliente';
import { ClienteService } from 'src/app/clientes/servicos/cliente.service';
import { Paginacao } from 'src/app/identidade-acesso/interfaces/paginacao';

@Component({
  selector: 'ca-procurar-cliente',
  templateUrl: './procurar-cliente.component.html',
  styleUrls: ['./procurar-cliente.component.css']
})
export class ProcurarClienteComponent implements OnInit {
  @Input() cliente!: VisualizarCliente;
  @Output() onClienteFormGroupChange = new EventEmitter();
  myControl = new FormControl<string | VisualizarCliente>('');
  clientesFiltrados!: Observable<VisualizarCliente[]>;
  clienteForm: FormGroup = {} as FormGroup;
  dataSource: Paginacao<VisualizarCliente> = {} as Paginacao<VisualizarCliente>;
  constructor(
    private fb: FormBuilder,
    private clienteService: ClienteService
  ) {
    this.dataSource.data = [];
  }

  ngOnInit(): void {
    this.clienteForm = this.fb.group({
      id: [''],
      nome: ['']
    });
    this.clienteForm.valueChanges.subscribe(() => this.onClienteFormGroupChange.emit(this.clienteForm.get('id')?.value));
    this.obterClientes();
    this.filtarClientes();
  }
  private filtarClientes() {
    this.clientesFiltrados = this.myControl.valueChanges.pipe(
      startWith(''),
      map((value: any) => {
        const nome = typeof value === 'string' ? value : value?.nome;
        return nome ? this._filtro(nome as string) : this.dataSource.data.slice();
      }));
  }
  obterClientes(
    pageIndex: number = 0,
    pageSize: number = 10
  ) {
    this.clienteService.obterCliente(pageIndex, pageSize).subscribe((x) => {
      this.dataSource = x;
    });
  }

  displayFn(cliente: VisualizarCliente[]): (id: number) => string {
    return (id: number) => {
      const opt = Array.isArray(this.dataSource.data)
        ? cliente.find((x) => x.id === id)
        : null;
      return opt ? opt.nome : '';
    };
  }
  private _filtro(value: string): VisualizarCliente[] {
    const filterValue = value.toLowerCase();
    return this.dataSource.data.filter((cliente) =>
      cliente.nome.toLowerCase().includes(filterValue)
    );
  }

  popularForm(id: number) {
    this.clienteService.ObterClientePorId(id).subscribe((cliente: VisualizarCliente) => {
      this.clienteForm.patchValue({
        id: cliente.id,
        // name: customer.name //não passar o nome, senão o input se esvazia, perde o nome
      })
    })

  }
}
