import { NovoEndereco } from './../../clientes/interfaces/ienderecos';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ICEP } from 'src/app/identidade-acesso/interfaces/cep';
import { BuscadorCepService } from 'src/app/identidade-acesso/servicos/buscador-cep.service';
import { SnackResponseService } from '../snack-response.service';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';

@Component({
  selector: 'ca-endereco-form',
  templateUrl: './endereco-form.component.html',
  styleUrls: ['./endereco-form.component.css']
})
export class EnderecoFormComponent implements OnInit {
  @Input() enderecoForm!: NovoEndereco;
  @Output() onEnderecoFormGroupChange = new EventEmitter();
  endereco: FormGroup = {} as FormGroup;
  constructor(
    private buscadorCepService: BuscadorCepService,
    private snackBar: SnackResponseService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.endereco = this.fb.group({
      cep: [this.enderecoForm.cep, [Validators.required, Validators.maxLength(9)]],
      logradouro: [this.enderecoForm.logradouro, [Validators.required, Validators.maxLength(255)]],
      numero: [this.enderecoForm.numero, [Validators.required, Validators.maxLength(50)]],
      bairro: [this.enderecoForm.bairro, [Validators.required, Validators.maxLength(255)]],
      cidade: [this.enderecoForm.cidade, [Validators.required, Validators.maxLength(255)]],
      complemento: [this.enderecoForm.complemento, [Validators.maxLength(255)]],
      uf: [this.enderecoForm.uf, [Validators.required, Validators.maxLength(11), Validators.pattern('[a-zA-Z]+')]],
    })
    this.endereco.valueChanges.subscribe(() => this.onEnderecoFormGroupChange.emit(this.endereco.value));
  }
  encontrarEndereco(cep: string) {
    this.buscadorCepService.getAddress(cep).subscribe((x: ICEP) => {
      this.endereco.patchValue({
        cep: x.cep,
        logradouro: x.logradouro,
        bairro: x.bairro,
        cidade: x.localidade,
        uf: x.uf
      })
    }, e => {
      this.snackBar.mostrarMensagem("Informe um CEP válido!", true);
    })
  }
  get cep() { return this.endereco.get('cep') as FormControl; }
  get logradouro() { return this.endereco.get('logradouro') as FormControl; }
  get numero() { return this.endereco.get('numero') as FormControl; }
  get bairro() { return this.endereco.get('bairro') as FormControl; }
  get cidade() { return this.endereco.get('cidade') as FormControl; }
  get uf() { return this.endereco.get('uf') as FormControl; }
  get complemento() { return this.endereco.get('complemento') as FormControl; }
}
