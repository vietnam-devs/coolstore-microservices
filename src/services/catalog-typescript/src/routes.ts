/* tslint:disable */
import { Controller, ValidateParam, FieldErrors, ValidateError, TsoaRoute } from 'tsoa';
import { ProductsController } from './controllers/productsController';

export function RegisterRoutes(app: any) {
  app.get('/v1/Products',
    function (request: any, response: any, next: any) {
      const controller = new ProductsController();

      const promise = controller.GetAll.apply(controller);
      promiseHandler(controller, promise, response, next);
    });
  app.get('/v1/Products/:productId',
    function (request: any, response: any, next: any) {
      const controller = new ProductsController();

      const promise = controller.Get.apply(controller);
      promiseHandler(controller, promise, response, next);
    });
  app.post('/v1/Users',
    function (request: any, response: any, next: any) {
      const controller = new ProductsController();


      const promise = controller.Create.apply(controller);
      promiseHandler(controller, promise, response, next);
    });
  app.delete('/v1/Users/:userId',
    function (request: any, response: any, next: any) {
      const controller = new ProductsController();


      const promise = controller.Delete.apply(controller);
      promiseHandler(controller, promise, response, next);
    });

  function promiseHandler(controllerObj: any, promise: any, response: any, next: any) {
    return Promise.resolve(promise)
      .then((data: any) => {
        let statusCode;
        if (controllerObj instanceof Controller) {
          const controller = controllerObj as Controller
          const headers = controller.getHeaders();
          Object.keys(headers).forEach((name: string) => {
            response.set(name, headers[name]);
          });

          statusCode = controller.getStatus();
        }

        if (data) {
          response.status(statusCode || 200).json(data);
        } else {
          response.status(statusCode || 204).end();
        }
      })
      .catch((error: any) => next(error));
  }
}
