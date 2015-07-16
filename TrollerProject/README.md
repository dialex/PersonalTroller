# Webservice Invoker v1.0

Este executável serve para fazer chamadas a webservices. A sua invocação deve ser agendada através do Scheduler do Windows.


## Funcionamento

Cada vez que o executável é corrido:

1. É lido o ficheiro `Tasks.txt` na mesma pasta do executável.
2. Cada linha é processada, uma de cada vez. Se ocorrer um erro, é escrito um ficheiro `Error.log`.
3. O executável termina. Se não ocorreram erros o ficheiro `Error.log` é apagado.


## Sintaxe do ficheiro `Tasks.txt`

Cada linha do ficheiro corresponde a uma tarefa ou ação:

- Linhas começadas por `//` são tratadas como comentários e ignoradas.
- Linhas começadas por `http` são intepretadas como definição do endpoint do WebService.
- As restantes linhas são concatenadas com o endpoint e o endereço resultante é invocado.

Exemplo:

```
http://SVRTEC01/loytyMailchimp/loytyMailChimp.asmx/
//Ping
SyncSubscribers
SendEmail?name=Teste&time=2015-06-30
http://SVRTEC01/loytyAPI/loytyAPI.asmx/
Ping
```

- A 1ª linha define o endereço onde está localizado o WebService (endpoint).
- A 2ª linha é ignorada.
- A 3ª linha invoca o WebMethod sem argumentos `http://SVRTEC01/loytyMailchimp/loytyMailChimp.asmx/SyncSubscribers`
- A 4ª linha invoca o WebMethod com argumentos `http://SVRTEC01/loytyMailchimp/loytyMailChimp.asmx/SendEmail?name=Teste&time=2015-06-30`
- A 5ª linha muda o endpoint a usar nas linhas seguintes
- A 6ª linha invoca o WebMethod sem argumentos `http://SVRTEC01/loytyAPI/loytyAPI.asmx/Ping`


## Resolução de problemas

1. É boa prática enviar todos os parâmetros no URL, mesmo que sejam vazios. Caso contrário o webservice pode retornar o erro 500 (Internal Server Error).