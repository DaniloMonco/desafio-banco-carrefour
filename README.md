# desafio-banco-carrefour





# Instruções



Na raiz do projeto existe um arquivo chamado setup.pdf com todas as instruções 



Existe também um arquivo chamado Teste Banco Carrefour.pdf com toda a documentação exigida no teste.



## Ambiente de Desenvolvimento

Abaixo segue uma breve descrição das quatro soluções criadas para resolver o teste. Foi utilizado .Net 10, linguagem C# e Visual Studio 2026 como ferramenta de desenvolvimento





**Opah.TransactionService** 

>API responsável pela entrada de lançamentos (débitos e créditos). Utiliza padrão outbox.



**Opah.TransactionOutbox**

>Worker responsável por processar a tabela de outbox e enviar as informações para o RabbitMQ



**Opah.ReportInbox**

>Worker responsável por ler as filas do RabbitMQ, traduzir esses eventos em um modelo e inserir no banco de dados PostgreSQL que será utilizado para geração dos relatórios.



**Opah.ReportService**

>API responsável por exibir o fluxo de caixa de acordo com a solicitação. Essa Api utiliza Redis para sistema de cache e lê os dados do PostgreSQL para geração dos relatórios.



