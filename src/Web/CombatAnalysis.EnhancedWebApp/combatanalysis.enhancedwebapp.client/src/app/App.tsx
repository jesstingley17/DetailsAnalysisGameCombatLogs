import { Route, Routes } from 'react-router-dom';
import AppRoutes from './Routes';
import Layout from '../shared/components/Layout';
import { AuthProvider } from '../shared/contexts/AuthProvider';

import './index.css';

const App: React.FC = () => {
    return (
        <AuthProvider>
            <Layout>
                <Routes>
                    {AppRoutes.map((route, index) => {
                        const { element, ...rest } = route;
                        return <Route key={index} {...rest} element={element} />;
                    })}
                </Routes>
            </Layout>
        </AuthProvider>
    );
}

export default App;