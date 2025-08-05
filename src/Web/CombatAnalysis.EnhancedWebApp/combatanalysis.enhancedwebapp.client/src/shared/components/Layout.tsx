import type { ReactNode } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

interface LayoutProps {
    children: ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
    const render = () => {
        return (
            <div>
                <NavMenu />
                <Container tag="main">
                  {children}
                </Container>
            </div>
        );
    }

    return render();
}

export default Layout;