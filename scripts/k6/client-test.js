import http from 'k6/http';
import { check } from 'k6';

export default function () {
  const payload = JSON.stringify({
    name: 'Novo Cliente',
    surname: 'Sobrenome',
    email: 'novo@cliente.com',
    birthDate: '1997-01-01'
  });
  
  const headers = { 'Content-Type': 'application/json' };

  let resPostClient = http.post('http://localhost:8080/client/api', payload, { headers: headers });
  check(resPostClient, {
    'status 201': (r) => r.status === 201,
    'response time was < 500ms': (r) => r.timings.duration < 500,
  });

  let newClient = JSON.parse(resPostClient.body);
  let newClientId = newClient.id;

  let resGetClientById = http.get(`http://localhost:8080/client/api/${newClientId}`);
  check(resGetClientById, {
    'status 200': (r) => r.status === 200,
    'response time was < 500ms': (r) => r.timings.duration < 500,
    'client has correct fields': (r) => {
      try {
        let client = JSON.parse(r.body);
        return (
          client.id === newClientId &&
          typeof client.name === 'string' &&
          typeof client.surname === 'string' &&
          typeof client.email === 'string' &&
          typeof client.birthDate === 'string' &&
          typeof client.createdAt === 'string' &&
          typeof client.updatedAt === 'string' &&
          typeof client.deleted === 'boolean'
        );
      } catch (e) {
        return false;
      }
    }
  });

  let resGetClients = http.get("http://localhost:8080/client/api");
  check(resGetClients, {
    'status 200': (r) => r.status === 200,
    'response time was < 500ms': (r) => r.timings.duration < 500,
    'response is an array of clients': (r) => {
      try {
        let body = JSON.parse(r.body);
        if (!Array.isArray(body)) {
          return false;
        }
        // Verifica se algum elemento do array tem o id desejado e se todos os campos estão corretos
        let isValid = body.every(element => {
          return (
            /^[\da-f]{8}-[\da-f]{4}-[\da-f]{4}-[\da-f]{4}-[\da-f]{12}$/i.test(element.id) &&
            typeof element.name === 'string' &&
            typeof element.surname === 'string' &&
            typeof element.email === 'string' &&
            typeof element.birthDate === 'string' &&
            typeof element.createdAt === 'string' &&
            typeof element.updatedAt === 'string' &&
            typeof element.deleted === 'boolean'
          );
        });
        return isValid;
      } catch (e) {
        return false;
      }
    }
  });

  const updatePayload = JSON.stringify({
    name: 'Cliente Atualizado',
    surname: 'Sobrenome Atualizado',
    email: 'atualizado@cliente.com',
    birthDate: '1997-01-02',
  });

  let resPutClient = http.put(`http://localhost:8080/client/api/${newClientId}`, updatePayload, { headers: headers });
  check(resPutClient, {
    'status 200': (r) => r.status === 200,
    'response time was < 500ms': (r) => r.timings.duration < 500,
  });

  resGetClientById = http.get(`http://localhost:8080/client/api/${newClientId}`);
  check(resGetClientById, {
    'status 200': (r) => r.status === 200,
    'response time was < 500ms': (r) => r.timings.duration < 500,
    'client has updated fields': (r) => {
      try {
        let client = JSON.parse(r.body);
        return (
          client.name === 'Cliente Atualizado' &&
          client.surname === 'Sobrenome Atualizado' &&
          client.email === 'atualizado@cliente.com' &&
          client.birthDate === '1985-01-01'
        );
      } catch (e) {
        return false;
      }
    }
  });

  let resDeleteClient = http.del(`http://localhost:8080/client/api/${newClientId}`);
  check(resDeleteClient, {
    'status 204': (r) => r.status === 204,
    'response time was < 500ms': (r) => r.timings.duration < 500,
  });

  resGetClientById = http.get(`http://localhost:8080/client/api/${newClientId}`);
  check(resGetClientById, {
    'status 404': (r) => r.status === 404,
  });
}
