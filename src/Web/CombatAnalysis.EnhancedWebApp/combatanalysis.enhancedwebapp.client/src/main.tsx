import { Suspense } from 'react';
import { createRoot } from 'react-dom/client';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import App from './app/App.tsx';
import Loading from './shared/components/Loading.tsx';
import Store from './app/Store.ts';

import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min';

import './i18n';

createRoot(document.getElementById('root')!).render(
    <Provider store={Store}>
        <BrowserRouter>
            <Suspense fallback={<Loading />}>
                <App />
            </Suspense>
        </BrowserRouter>
    </Provider>

)