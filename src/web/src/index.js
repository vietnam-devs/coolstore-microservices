import './style';
import { h, render } from 'preact';
import App from './components/app';

const mountNode = document.getElementById('app');
render(<App />, mountNode, mountNode.lastChild);
