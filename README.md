# Cashflow.API 💰

Bem-vindo ao **Cashflow.API**! Este projeto foi desenvolvido para facilitar o registro e a gestão de despesas, bem como a geração de relatórios em PDF e Excel. O projeto foi construído seguindo os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**.

## Funcionalidades 🚀

- **Registrar Despesas**: Permite aos usuários registrar suas despesas de forma simples e eficiente.
- **Gerar Relatórios em PDF e Excel**: Gera relatórios das despesas registradas em formatos PDF e Excel para fácil visualização e compartilhamento.

## Tecnologias Utilizadas 🔧

- **Arquitetura Limpa (Clean Architecture)**
- **Domain-Driven Design (DDD)**
- **Entity Framework Core**: Para interação com o banco de dados.
- **AutoMapper**: Para mapeamento entre entidades e DTOs.
- **ClosedXML**: Para geração de arquivos Excel.
- **MigraDock**: Para geração de relatórios em PDF.
- **XUnit**: Para testes unitários.
- **Bogus**: Para geração de dados falsos durante os testes.
- **FluentAssertions**: Para uma sintaxe mais clara em asserções nos testes.
- **FluentValidation**: Para validação de dados de entrada.

## Estrutura do Projeto 📂

O projeto está organizado em várias camadas, seguindo a arquitetura limpa:

- **API**: A camada de apresentação que expõe os endpoints HTTP.
- **Application**: Contém casos de uso e lógica de aplicação.
- **Domain**: Contém as entidades e os domínios de negócio.
- **Exception**: Implementação de tratamento de exceções personalizadas para uma gestão adequada de erros, garantindo uma resposta consistente e informativa em caso de falhas.
- **Infrastructure**: Implementações de acesso a dados e serviços externos.
- **Test**: Contém os testes unitários e de integração para garantir a qualidade do código e a funcionalidade do sistema.

