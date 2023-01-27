import { TipoDocumento } from 'src/app/clientes/enum/tipoDocumento';

export interface AutoCadastro {
  documento: string;
  tipoDocumento: TipoDocumento;
  nome: string;
  dataNascimento: Date;
  cep: string;
  logradouro: string;
  numero: string;
  complemento: string;
  bairro: string;
  cidade: string;
  uf: string;
  telefone: string;
  contato: string;
  email: string;
  observacao: string;
}
