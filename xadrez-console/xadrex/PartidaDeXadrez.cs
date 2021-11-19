using System;
using System.Collections.Generic;
using System.Text;
using tabuleiro;

namespace xadrex
{
    class PartidaDeXadrez
    {
        public Tabuleiro tab { get; private set; }
        public int turno { get; private set;  }
        public Cor jogadorAtual { get; private set; }
        public bool terminada { get; private set; }
        private HashSet<Peca> pecas;
        private HashSet<Peca> capturadas;

        public PartidaDeXadrez()
        {
            this.tab = new Tabuleiro(8, 8);
            this.turno = 1;
            this.jogadorAtual = Cor.Branco;
            terminada = false;
            pecas = new HashSet<Peca>();
            capturadas = new HashSet<Peca>();
            colocarPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = tab.retirarPeca(origem);
            p.incrementarQteMovimentos();
            Peca pecaCapturada = tab.retirarPeca(destino);
            tab.colocarPeca(p, destino);
            if (pecaCapturada != null)
            {
                capturadas.Add(pecaCapturada);
            }
            mudaJogador();
        }

        public void realizaJogada(Posicao origem, Posicao destino)
        {
            ExecutaMovimento(origem, destino);
            turno++;
            mudaJogador();
        }

        public void validarPosicaoDeOrigem(Posicao pos)
        {
            if (tab.peca(pos) == null)
            {
                throw new TabuleiroException("Nao existe peca na posicao de origem escolhida!");
            }
            if (jogadorAtual != tab.peca(pos).cor)
            {
                throw new TabuleiroException("A peca de origem escolhida nao e a sua");
            }
            if (!tab.peca(pos).existeMovimentosPossiveis())
            {
                throw new TabuleiroException("Nao ha movimentos possiveis para a peca de origem escolhida!");
            }
        }

        public void validarPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!tab.peca(origem).podeMoverPara(destino))
            {
                throw new TabuleiroException("Posicao de destino invalida");
            }
        }

        private void mudaJogador()
        {
            if (jogadorAtual == Cor.Branco)
            {
                jogadorAtual = Cor.Preto;
            }
            else
            {
                jogadorAtual = Cor.Branco;
            }
        }

        public HashSet<Peca> pecasCapturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in capturadas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> pecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in pecas)
            {
                if (x.cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(pecasCapturadas(cor));
            return aux;
        }

        public void colocarNovaPeca(char coluna, int linha, Peca peca)
        {
            tab.colocarPeca(peca, new PosicaoXadrez(coluna, linha).toPosicao());
            pecas.Add(peca);
        }

        private void colocarPecas()
        {

            colocarNovaPeca('c', 1, new Torre(tab, Cor.Branco));
            colocarNovaPeca('c', 2, new Torre(tab, Cor.Branco));
            colocarNovaPeca('d', 2, new Torre(tab, Cor.Branco));
            colocarNovaPeca('e', 2, new Torre(tab, Cor.Branco));
            colocarNovaPeca('e', 1, new Torre(tab, Cor.Branco));
            colocarNovaPeca('d', 1, new Rei(tab, Cor.Branco));

            colocarNovaPeca('c', 7, new Torre(tab, Cor.Preto));
            colocarNovaPeca('c', 8, new Torre(tab, Cor.Preto));
            colocarNovaPeca('d', 7, new Torre(tab, Cor.Preto));
            colocarNovaPeca('e', 7, new Torre(tab, Cor.Preto));
            colocarNovaPeca('e', 8, new Torre(tab, Cor.Preto));
            colocarNovaPeca('d', 8, new Rei(tab, Cor.Preto));
            
        }
    }
}
