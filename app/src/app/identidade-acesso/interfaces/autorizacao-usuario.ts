export interface AutorizacaoUsuario {
  email: string;
  senha: string;
}

export interface RecuperarSenha {
  email: string;
}

export interface AlterarSenha {
  novaSenha: string;
  confirmarNovaSenha: string;
}
