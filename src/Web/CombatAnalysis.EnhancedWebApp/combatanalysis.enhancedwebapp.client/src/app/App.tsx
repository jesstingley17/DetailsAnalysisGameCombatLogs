import ChatHubProvider from '@/context/ChatHubProvider';
import Layout from '@/shared/components/Layout';
import { AuthProvider } from '@/context/AuthProvider';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './Routes';

import './App.css';

const App: React.FC = () => {
    return (
        <AuthProvider>
            <Layout>
                <ChatHubProvider>
                    <Routes>
                        {AppRoutes.map((route, index) => {
                            const { element, ...rest } = route;
                            return <Route key={index} {...rest} element={element} />;
                        })}
                    </Routes>
                </ChatHubProvider>
            </Layout>
        </AuthProvider>
    );
}

export default App;