/* tslint:disable */
import { Controller, ValidateParam, FieldErrors, ValidateError, TsoaRoute } from 'tsoa';
import { ProductController } from './controllers/productController';

const models: TsoaRoute.Models = {
  "ProductCreateRequest": {
    "properties": {
      "name": { "dataType": "string", "required": true },
      "desc": { "dataType": "string", "required": true },
      "price": { "dataType": "double", "required": true },
      "imageUrl": { "dataType": "string", "required": true },
    },
  },
};

export function RegisterRoutes(app: any) {
  app.get('/api/products',
    function(request: any, response: any, next: any) {
      const args = {
        currentPage: { "default": 0, "in": "query", "name": "currentPage", "dataType": "double" },
        highPrice: { "in": "query", "name": "highPrice", "dataType": "double" },
      };

      let validatedArgs: any[] = [];
      try {
        validatedArgs = getValidatedArgs(args, request);
      } catch (err) {
        return next(err);
      }

      const controller = new ProductController();


      const promise = controller.GetAll.apply(controller, validatedArgs);
      promiseHandler(controller, promise, response, next);
    });
  app.get('/api/products/:productId',
    function(request: any, response: any, next: any) {
      const args = {
        productId: { "in": "path", "name": "productId", "required": true, "dataType": "string" },
      };

      let validatedArgs: any[] = [];
      try {
        validatedArgs = getValidatedArgs(args, request);
      } catch (err) {
        return next(err);
      }

      const controller = new ProductController();


      const promise = controller.Get.apply(controller, validatedArgs);
      promiseHandler(controller, promise, response, next);
    });
  app.post('/api/products',
    function(request: any, response: any, next: any) {
      const args = {
        request: { "in": "body", "name": "request", "required": true, "ref": "ProductCreateRequest" },
      };

      let validatedArgs: any[] = [];
      try {
        validatedArgs = getValidatedArgs(args, request);
      } catch (err) {
        return next(err);
      }

      const controller = new ProductController();


      const promise = controller.Create.apply(controller, validatedArgs);
      promiseHandler(controller, promise, response, next);
    });


  function isController(object: any): object is Controller {
    return 'getHeaders' in object && 'getStatus' in object && 'setStatus' in object;
  }

  function promiseHandler(controllerObj: any, promise: any, response: any, next: any) {
    return Promise.resolve(promise)
      .then((data: any) => {
        let statusCode;
        if (isController(controllerObj)) {
          const headers = controllerObj.getHeaders();
          Object.keys(headers).forEach((name: string) => {
            response.set(name, headers[name]);
          });

          statusCode = controllerObj.getStatus();
        }

        if (data || data === false) { // === false allows boolean result
          response.status(statusCode || 200).json(data);
        } else {
          response.status(statusCode || 204).end();
        }
      })
      .catch((error: any) => next(error));
  }

  function getValidatedArgs(args: any, request: any): any[] {
    const fieldErrors: FieldErrors = {};
    const values = Object.keys(args).map((key) => {
      const name = args[key].name;
      switch (args[key].in) {
        case 'request':
          return request;
        case 'query':
          return ValidateParam(args[key], request.query[name], models, name, fieldErrors);
        case 'path':
          return ValidateParam(args[key], request.params[name], models, name, fieldErrors);
        case 'header':
          return ValidateParam(args[key], request.header(name), models, name, fieldErrors);
        case 'body':
          return ValidateParam(args[key], request.body, models, name, fieldErrors, name + '.');
        case 'body-prop':
          return ValidateParam(args[key], request.body[name], models, name, fieldErrors, 'body.');
      }
    });
    if (Object.keys(fieldErrors).length > 0) {
      throw new ValidateError(fieldErrors, '');
    }
    return values;
  }
}
