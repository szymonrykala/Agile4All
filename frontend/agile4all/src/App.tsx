import * as React from 'react';
import SideNav from './components/SideNav'
import Layout from './components/Layout';
import Header from './components/Header';
import { Outlet } from 'react-router-dom';
import { useAppSelector } from './hooks';
import { ChatContextProvider } from './components/Chat/Context';
import Chat from './components/Chat';
import ResourceLoader from './components/common/ReourceLoader';


export default function App() {
  const [drawerOpen, setDrawerOpen] = React.useState(false);
  const session = useAppSelector(({ session }) => session);

  return (
    <>
      {session && <ResourceLoader />}

      <Layout.Root>
        <ChatContextProvider>

          <Layout.Header >
            <Header
              setDrawerOpen={() => setDrawerOpen(!drawerOpen)}
              session={session}
            />
          </Layout.Header>

          <Layout.SideNav isOpen={drawerOpen}>
            <SideNav />
          </Layout.SideNav>

          <Layout.Main>
            <Outlet />
          </Layout.Main>

          <Chat />

        </ChatContextProvider>
      </Layout.Root>
    </>
  );
}