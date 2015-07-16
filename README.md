# PersonalTroller

I created

## How to use

1. ...

## How it works


## Example

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

Feel free to add new trolling functionality and send a pull request.