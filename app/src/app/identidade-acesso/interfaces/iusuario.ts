import { Funcao } from "../enum/funcao";
export interface NovoUsuario {
    nome: string;
    email: string;
    funcao: Funcao;
}
export interface VisualizarUsuario {
    id: number;
    nome: string;
    email: string;
    ativo: boolean;
    funcao: Funcao;
}
export interface AlterarUsuario {
    id: number;
    nome: string;
    email: string;
    ativo: boolean;
    funcao: Funcao;
}
