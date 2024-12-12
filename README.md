# desafio-banco-carrefour

# Instruções

Caro avaliador, devido ao tempo não consegui escrever um docker compose com todos os componentes necessários. Sendo bem sincero, a maioria dos containers já existiam na minha máquina e aproveitei para me concentrar na arquitetura e desenvolvimento do código, onde realmente valia medir esforços na solução/desafio. Vou deixar os comandos docker para rodar individualmente em cada aba do PowerShell, imagino que isso será o suficiente.

## RabbitMQ
docker run --rm -it -p 15672:15672 -p 5672:5672 rabbitmq:3-management

Acessar o management pelo endereço http://localhost:15672/#/  onde usuário e senha é **guest**
Criar a fila **credito-lancado-queue** do tipo *Durability = Durable*
Criar a fila **debito-lancado-queue** do tipo *Durability = Durable*

Criar a exchange **credito.lancado** do *Type = fanout* and *Durability = Durable*
Entrar no credito.lancado e na area **Add binding from this exchange**, informar **To queue = credito-lancado-queue** e clicar em **Bind**

Criar a exchange **debito.lancado** do *Type = fanout* and *Durability = Durable*
Entrar no debito.lancado e na area **Add binding from this exchange**, informar **To queue = debito-lancado-queue** e clicar em **Bind**


## PostgreSQL
docker run --name postgresql -e POSTGRES_USER=myusername -e POSTGRES_PASSWORD=mypassword -p 5432:5432 -v /data:/var/lib/postgresql/data -d postgres
O usuario para acessar o postgresql é **myusername** e a senha é **mypassword**, conforme comando do docker.

Criar o banco de dados chamado **fluxo-caixa** e a seguinte tabela

    create table lancamentos
    (
    	Id UUID not null primary key, 
    	DataHora timestamp not null, 
    	Valor decimal(14,2) not null, 
    	Descricao varchar(100) not null, 
    	TipoLancamento char(1) not null
    )


## Redis
docker run --name redis -p 6379:6379 -d redis

## EventStore
docker run --name esdb-node -it -p 2113:2113 -p 1113:1113 eventstore/eventstore:latest --insecure --run-projections=All

Acessar o management pelo endereço http://localhost:2113/

## Ambiente de Desenvolvimento
Abaixo segue uma breve descrição das três soluções criadas para resolver o teste. Foi utilizado .Net 8, linguagem C# e Visual Studio 2022 como ferramenta de desenvolvimento


**ControleLancamento.Api** 
>API responsável pela entrada de lançamentos (débitos e créditos). Utiliza Rabbitmq para envio dos eventos e EventStore como base de dados para armazenar os eventos.

**FluxoCaixaBackground**
>Serviço batch responsável por ler as filas do RabbitMQ (ou seja, ler os eventos gerados pelo ControleLancamento.Api), traduzir esses eventos em um modelo e inserir no banco de dados PostgreSQL que será utilizado para geração dos relatórios.

**FluxoCaixa.Api**
>API responsável por exibir o fluxo de caixa de acordo com a solicitação. Essa Api utiliza Redis para sistema de cache e lê os dados do PostgreSQL para geração dos relatórios.
