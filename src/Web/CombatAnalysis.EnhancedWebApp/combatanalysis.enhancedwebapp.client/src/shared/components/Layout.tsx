import type { ReactNode } from 'react';
import { Container } from 'reactstrap';
import NavMenu from './NavMenu';

const Layout: React.FC<{ children: ReactNode }> = ({ children }) => {
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