import './controllers/productsController';
import * as bodyParser from 'body-parser';
import * as express from 'express';
import * as methodOverride from 'method-override';
import { RegisterRoutes } from './routes';
const swaggerJSON = require('./swagger.json');
import * as swaggerUI from "swagger-ui-express";

const app = express();

let basePath = process.env.BASE_PATH;
if (!basePath) {
  basePath = '/';
}
console.log(`Base path is ${basePath}`);

app.use('/swagger.json', express.static(__dirname + './swagger.json'));

app.use('/', swaggerUI.serve, swaggerUI.setup(swaggerJSON));

app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());
app.use(methodOverride());

RegisterRoutes(app);

/* tslint:disable-next-line */
app.listen(process.env.PORT || 5002, () => {
  // migrate for the first time when started
  // Product.findProducts().then(products => {
  //   console.info(products)
  //   if (products.length <= 0) {
  //     var seeds = [
  //       {
  //         id: 'ba16da71-c7dd-4eac-9ead-5c2c2244e69f',
  //         name: 'IPhone 8',
  //         desc: 'IPhone 8',
  //         price: 12.5,
  //         imageUrl: 'https://picsum.photos/400/300?image=0'
  //       },
  //       {
  //         id: '13d02035-2286-4055-ad2d-6855a60efbbb',
  //         name: 'IPhone X',
  //         desc: 'IPhone X',
  //         price: 20.5,
  //         imageUrl: 'https://picsum.photos/400/300?image=1'
  //       },
  //       {
  //         id: 'b8f0a771-339f-4602-a862-f7a51afd5b79',
  //         name: 'MacBook Pro 2019',
  //         desc: 'MacBook Pro 2019',
  //         price: 15.3,
  //         imageUrl: 'https://picsum.photos/400/300?image=2'
  //       }
  //     ]

  //     seeds.map(x => {
  //       var newProduct = new Product()
  //       newProduct.createProduct(x)
  //     })
  //   }
  // })
  console.info(`App's running at http://localhost:${process.env.PORT || 5002}`);
  console.info('Press CTRL-C to stop\n');
});
