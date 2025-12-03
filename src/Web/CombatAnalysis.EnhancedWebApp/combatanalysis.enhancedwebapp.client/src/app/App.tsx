import { AuthProvider } from '@/context/AuthProvider';
import ChatHubProvider from '@/context/ChatHubProvider';
import NotificationProvider from '@/context/NotificationProvider';
import Layout from '@/shared/components/Layout';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './Routes';

import './App.css';

const App: React.FC = () => {
    return (
        <AuthProvider>
            <NotificationProvider>
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
            </NotificationProvider>
        </AuthProvider>
    );
}

export default App;